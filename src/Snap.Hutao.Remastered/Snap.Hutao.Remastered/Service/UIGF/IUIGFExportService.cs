// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Service.UIGF;

public interface IUIGFExportService
{
    ValueTask ExportAsync(UIGFExportOptions exportOptions, CancellationToken token);
}