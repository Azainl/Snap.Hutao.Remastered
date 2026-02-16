// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Web.Bridge.Model;

public sealed class PushPagePayload
{
    [JsonPropertyName("page")]
    public string Page { get; set; } = default!;
}