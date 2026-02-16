// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Model.Metadata.Reliquary;
using System.Collections.Immutable;

namespace Snap.Hutao.Remastered.Service.Metadata.ContextAbstraction.ImmutableArray;

public interface IMetadataArrayReliquarySource
{
    ImmutableArray<Reliquary> Reliquaries { get; set; }
}