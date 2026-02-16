// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Model;
using Snap.Hutao.Remastered.Model.Calculable;
using Snap.Hutao.Remastered.Model.Intrinsic;
using Snap.Hutao.Remastered.Model.Primitive;
using System.Collections.Immutable;

namespace Snap.Hutao.Remastered.ViewModel.AvatarProperty;

public sealed class WeaponView : EquipView, ICalculableSource<ICalculableWeapon>
{
    public NameValue<string>? SubProperty { get; set; }

    public uint AffixLevelNumber { get; set; }

    public string AffixLevel { get => SH.FormatModelBindingAvatarPropertyWeaponAffix(AffixLevelNumber); }

    public string AffixName { get; set; } = default!;

    public string AffixDescription { get; set; } = default!;

    public ImmutableArray<bool> PromoteArray { get; set; }

    public WeaponId Id { get; set; }

    public uint LevelNumber { get; set; }

    public uint MaxLevel { get => Model.Metadata.Weapon.Weapon.GetMaxLevelByQuality(Quality); }

    public WeaponType WeaponType { get; set; }

    public PromoteLevel PromoteLevel { get; set; }

    public ICalculableWeapon ToCalculable()
    {
        return CalculableWeapon.From(this);
    }
}