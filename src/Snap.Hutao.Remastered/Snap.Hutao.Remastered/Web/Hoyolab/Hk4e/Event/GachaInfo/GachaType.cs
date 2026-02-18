// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Web.Hoyolab.Hk4e.Event.GachaInfo;

[ExtendedEnum]
public enum GachaType
{
    [LocalizationKey(nameof(SH.WebGachaConfigTypeNoviceWish))]
    NewBie = 100,

    [LocalizationKey(nameof(SH.WebGachaConfigTypePermanentWish))]
    Standard = 200,

    [LocalizationKey(nameof(SH.WebGachaConfigTypeAvatarEventWish))]
    ActivityAvatar = 301,

    [LocalizationKey(nameof(SH.WebGachaConfigTypeWeaponEventWish))]
    ActivityWeapon = 302,

    [LocalizationKey(nameof(SH.WebGachaConfigTypeAvatarEventWish2))]
    SpecialActivityAvatar = 400,

    [LocalizationKey(nameof(SH.WebGachaConfigTypeChronicledWish))]
    ActivityCity = 500,

    // UGC

    [LocalizationKey(nameof(SH.WebUGCGachaConfigTypePermanentWish))]
    UGCStandard = 1000,

    [LocalizationKey(nameof(SH.WebUGCGachaConfigTypeAvatarEventWish))]
    UGCAvatarEventWish = 2000,

    [LocalizationKey(nameof(SH.WebUGCGachaConfigTypeActivityAvatarMaleOne))]
    UGCActivityAvatarMaleOne = 20011,

    [LocalizationKey(nameof(SH.WebUGCGachaConfigTypeActivityAvatarMaleTwo))]
    UGCActivityAvatarMaleTwo = 20012,

    [LocalizationKey(nameof(SH.WebUGCGachaConfigTypeActivityAvatarFemaleOne))]
    UGCActivityAvatarFemaleOne = 20021,

    [LocalizationKey(nameof(SH.WebUGCGachaConfigTypeActivityAvatarFemaleTwo))]
    UGCActivityAvatarFemaleTwo = 20022
}