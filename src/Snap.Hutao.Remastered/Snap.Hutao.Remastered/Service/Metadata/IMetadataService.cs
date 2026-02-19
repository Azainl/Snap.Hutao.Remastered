// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Extensions.Caching.Memory;
using Snap.Hutao.Remastered.Model.Metadata.Item;
using Snap.Hutao.Remastered.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Remastered.Service.Metadata;

public interface IMetadataService : IMetadataServiceInitialization
{
    IMemoryCache MemoryCache { get; }

    ValueTask<ImmutableArray<T>> FromCacheOrFileAsync<T>(MetadataFileStrategy strategy, CancellationToken token)
        where T : class;

    ValueTask<bool> InitializeAsync();
}