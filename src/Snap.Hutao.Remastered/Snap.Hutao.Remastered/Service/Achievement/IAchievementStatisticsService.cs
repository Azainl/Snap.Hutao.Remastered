// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.ViewModel.Achievement;
using System.Collections.Immutable;

namespace Snap.Hutao.Remastered.Service.Achievement;

public interface IAchievementStatisticsService
{
    ValueTask<ImmutableArray<AchievementStatistics>> GetAchievementStatisticsAsync(AchievementServiceMetadataContext context, CancellationToken token = default);
}