using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Snap.Hutao.Remastered.Model.InterChange.GachaLog;

// This class unfortunately can't use required properties because it's been rooted in XamlTypeInfo
// ReSharper disable once InconsistentNaming
public class UIGF42 : UIGF
{
    // ReSharper disable once InconsistentNaming
    [JsonPropertyName("hkrpg")]
    public ImmutableArray<UIGFEntry<Hk4eItem>> Hkrpg { get; set; } = [];

    // ReSharper disable once InconsistentNaming
    [JsonPropertyName("hk4e_ugc")]
    public ImmutableArray<UIGFEntry<Hk4eUGCItem>> Hk4eUgc { get; set; } = [];
}
