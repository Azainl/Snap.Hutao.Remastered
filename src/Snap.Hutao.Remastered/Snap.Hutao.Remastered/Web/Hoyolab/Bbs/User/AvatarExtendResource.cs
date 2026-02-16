// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Web.Hoyolab.Bbs.User;

public sealed class AvatarExtendResource
{
    [JsonPropertyName("format")]
    public required int Format { get; init; }

    [JsonPropertyName("url")]
    public required Uri Url { get; init; }
}