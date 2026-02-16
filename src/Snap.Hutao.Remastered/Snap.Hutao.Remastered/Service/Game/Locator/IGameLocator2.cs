// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Remastered.Service.Game.Locator;

public interface IGameLocator2
{
    ValueTask<ImmutableArray<string>> LocateMultipleGamePathAsync();
}