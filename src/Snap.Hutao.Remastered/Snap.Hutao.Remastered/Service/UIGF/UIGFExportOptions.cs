// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Immutable;

namespace Snap.Hutao.Remastered.Service.UIGF;

public sealed class UIGFExportOptions
{
    public required string FilePath { get; set; }

    public required ImmutableArray<uint> GachaArchiveUids { get; set; }

    public UIGFVersion Version { get; set; } = UIGFVersion.UIGF42;
}
