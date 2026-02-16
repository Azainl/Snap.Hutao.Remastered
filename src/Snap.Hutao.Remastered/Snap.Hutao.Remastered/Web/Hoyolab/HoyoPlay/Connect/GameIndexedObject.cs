// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Web.Hoyolab.HoyoPlay.Connect;

public abstract class GameIndexedObject
{
    [JsonPropertyName("game")]
    public Game Game { get; set; } = default!;
}
