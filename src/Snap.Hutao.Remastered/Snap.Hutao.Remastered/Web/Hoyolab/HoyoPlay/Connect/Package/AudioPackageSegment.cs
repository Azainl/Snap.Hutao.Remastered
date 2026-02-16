// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Web.Hoyolab.HoyoPlay.Connect.Package;

public sealed class AudioPackageSegment : PackageSegment
{
    [JsonPropertyName("language")]
    public string Language { get; set; } = default!;
}
