// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Core.Database;
using Snap.Hutao.Remastered.Model.Entity;
using Snap.Hutao.Remastered.Service.GachaLog.QueryProvider;
using Snap.Hutao.Remastered.Web.Hoyolab.Hk4e.Event.GachaInfo;
using System.Collections.Immutable;

namespace Snap.Hutao.Remastered.Service.GachaLog;

public sealed class BeyondGachaLogFetchContext
{
    private readonly GachaLogServiceMetadataContext serviceContext;
    private readonly IGachaLogRepository repository;
    private readonly bool isLazy;

    public BeyondGachaLogFetchContext(IGachaLogRepository repository, GachaLogServiceMetadataContext serviceContext, bool isLazy)
    {
        this.repository = repository;
        this.serviceContext = serviceContext;
        this.isLazy = isLazy;
    }

    public GachaLogFetchStatus Status { get; set; } = default!;

    public List<BeyondGachaItem> ItemsToAdd { get; set; } = [];

    public GachaArchive? TargetArchive { get; set; }

    public long? DbEndId { get; set; }

    public GachaLogTypedQueryOptions TypedQueryOptions { get; set; } = default!;

    public bool CurrentTypeAddingCompleted { get; set; }

    public GachaType CurrentType { get; set; }

    public void ResetType(GachaType configType, in GachaLogQuery query)
    {
        DbEndId = null;
        CurrentType = configType;
        ItemsToAdd.Clear();
        Status = new(configType);
        TypedQueryOptions = new(query, configType);
        CurrentTypeAddingCompleted = false;
    }

    public void ResetCurrentPage()
    {
        Status = new(CurrentType);
    }

    public void EnsureArchiveAndEndId(BeyondGachaLogItem item, IAdvancedDbCollectionView<GachaArchive> archives, IGachaLogRepository repository)
    {
        TargetArchive ??= GachaArchiveOperation.GetOrAdd(repository, item.Uid, archives);
        DbEndId ??= repository.GetNewestBeyondGachaItemIdByArchiveIdAndGachaType(TargetArchive.InnerId, CurrentType);
    }

    public bool ShouldAddItem(BeyondGachaLogItem item)
    {
        // For non-lazy mode, all items should be added
        return !isLazy || item.Id > DbEndId; // DbEndId will be evaluated to 0 if null
    }

    public bool HasReachCurrentTypeEnd(ImmutableArray<BeyondGachaLogItem> items)
    {
        return CurrentTypeAddingCompleted || items.Length < GachaLogTypedQueryOptions.Size;
    }

    public void AddItem(BeyondGachaLogItem item)
    {
        ArgumentNullException.ThrowIfNull(TargetArchive);
        ItemsToAdd.Add(BeyondGachaItem.From(TargetArchive.InnerId, item));
        // For beyond gacha items, we don't have a direct way to get the item from serviceContext
        // So we'll add a placeholder item or skip adding to status
        TypedQueryOptions.EndId = item.Id;
    }

    public void SaveItems()
    {
        if (ItemsToAdd.Count <= 0)
        {
            return;
        }

        if (TargetArchive is null)
        {
            return;
        }

        if (!isLazy)
        {
            // Aggressive mode: Remove all items of the same type and newer than end id
            repository.RemoveBeyondGachaItemRangeByArchiveIdAndGachaTypeNewerThanEndId(TargetArchive.InnerId, TypedQueryOptions.Type, TypedQueryOptions.EndId);
        }

        repository.AddBeyondGachaItemRange(ItemsToAdd);
    }

    public void CompleteCurrentTypeAdding()
    {
        CurrentTypeAddingCompleted = true;
    }

    public void Report(IProgress<GachaLogFetchStatus> progress, bool isAuthKeyTimeout = false)
    {
        Status.AuthKeyTimeout = isAuthKeyTimeout;
        progress.Report(Status);
    }
}
