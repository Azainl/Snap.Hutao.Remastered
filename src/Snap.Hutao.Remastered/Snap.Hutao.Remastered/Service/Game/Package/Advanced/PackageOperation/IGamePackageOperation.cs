// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Service.Game.Package.Advanced.PackageOperation;

public interface IGamePackageOperation
{
    ValueTask ExecuteAsync(GamePackageServiceContext context);
}