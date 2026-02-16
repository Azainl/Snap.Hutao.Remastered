// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Web.Hoyolab.HoyoPlay.Connect.Branch;

public sealed class GameBranch : GameIndexedObject
{
    [JsonPropertyName("main")]
    public BranchWrapper Main { get; set; } = default!;

    [JsonPropertyName("pre_download")]
    public BranchWrapper? PreDownload { get; set; }
}