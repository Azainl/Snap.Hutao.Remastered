// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Web.Bridge.Model;

public sealed class CookieTokenPayload
{
    [JsonPropertyName("forceRefresh")]
    public bool ForceRefresh { get; set; }
}