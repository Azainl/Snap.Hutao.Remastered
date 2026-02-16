// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Web.Hoyolab.HoyoPlay.Connect.Package;

public sealed class GamePackagesWrapper
{
    [JsonPropertyName("game_packages")]
    public List<GamePackage> GamePackages { get; set; } = default!;
}
