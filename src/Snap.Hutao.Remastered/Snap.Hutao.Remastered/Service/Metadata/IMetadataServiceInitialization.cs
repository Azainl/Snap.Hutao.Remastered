// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Service.Metadata;

public interface IMetadataServiceInitialization
{
    ValueTask InitializepublicAsync(CancellationToken token = default);
}