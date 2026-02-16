// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Service.Game.Locator;

public interface IGameLocatorFactory
{
    IGameLocator Create(GameLocationSourceKind source);
}
