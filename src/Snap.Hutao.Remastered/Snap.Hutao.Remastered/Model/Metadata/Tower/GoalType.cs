// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Model.Metadata.Tower;

[ExtendedEnum]
public enum GoalType
{
    [LocalizationKey(nameof(SH.ModelMetadataTowerGoalTypeDefeatMonsters))]
    DefeatMonsters,

    [LocalizationKey(nameof(SH.ModelMetadataTowerGoalTypeDefendTarget))]
    DefendTarget,
}