// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Web.Hutao.Strategy;

public sealed class Strategy
{
    [JsonPropertyName("mys_strategy_id")]
    public int? MysStrategyId { get; set; }

    [JsonPropertyName("hoyolab_strategy_id")]
    public int? HoyolabStrategyId { get; set; }
}