// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Model.InterChange.GachaLog;
using Snap.Hutao.Remastered.Web.Hoyolab.Hk4e.Event.GachaInfo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Hutao.Remastered.Model.Entity;

[Table("beyond_gacha_items")]
public sealed class BeyondGachaItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid InnerId { get; set; }

    [ForeignKey(nameof(ArchiveId))]
    public GachaArchive Archive { get; set; } = default!;

    public Guid ArchiveId { get; set; }

    public GachaType GachaType { get; set; }

    public uint ItemId { get; set; }

    public DateTimeOffset Time { get; set; }

    public long Id { get; set; }

    public string ItemType { get; set; } = default!;

    public string ItemName { get; set; } = default!;

    public long ScheduleId { get; set; }

    public long RankType { get; set; }

    public static BeyondGachaItem From(Guid archiveId, BeyondGachaLogItem item)
    {
        return new()
        {
            ArchiveId = archiveId,
            GachaType = item.GachaType,
            ItemId = uint.Parse(item.ItemId),
            Time = item.Time,
            Id = item.Id,
            ItemType = item.ItemType,
            ItemName = item.ItemName,
            ScheduleId = long.Parse(item.ScheduleId),
            RankType = (long)item.RankType,
        };
    }

    public static BeyondGachaItem From(Guid archiveId, Hk4eUGCItem item, int timezoneOffset)
    {
        return new()
        {
            ArchiveId = archiveId,
            GachaType = item.GachaType,
            ItemId = item.ItemId,
            Time = new(item.Time, TimeSpan.FromHours(timezoneOffset)),
            Id = item.Id,
            ItemType = item.ItemType,
            ItemName = item.ItemName,
            ScheduleId = item.ScheduleId,
            RankType = item.RankType,
        };
    }

    public Hk4eUGCItem ToHk4eUGCItem()
    {
        return new()
        {
            GachaType = GachaType,
            ItemId = ItemId,
            Time = Time.UtcDateTime,
            Id = Id,
            ItemType = ItemType,
            ItemName = ItemName,
            ScheduleId = ScheduleId,
            RankType = RankType,
        };
    }
}
