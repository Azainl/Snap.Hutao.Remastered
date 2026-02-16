// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Service.UIGF;

public sealed class UIGFImportOptions
{
    public required Model.InterChange.GachaLog.UIGF UIGF { get; set; }

    public required HashSet<uint> GachaArchiveUids { get; set; }
}