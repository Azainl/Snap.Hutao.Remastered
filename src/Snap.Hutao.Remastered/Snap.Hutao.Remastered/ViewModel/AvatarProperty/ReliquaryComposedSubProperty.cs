// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Model.Intrinsic;

namespace Snap.Hutao.Remastered.ViewModel.AvatarProperty;

public class ReliquaryComposedSubProperty : ReliquarySubProperty
{
    public ReliquaryComposedSubProperty(FightProperty type, string value, uint enhancedCount)
        : base(type, value)
    {
        Type = type;
        EnhancedCount = enhancedCount;
    }

    public uint EnhancedCount { get; set; }

    public FightProperty Type { get; }
}