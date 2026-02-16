// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Service.GachaLog.QueryProvider;

public interface IGachaLogQueryProvider
{
    ValueTask<ValueResult<bool, GachaLogQuery>> GetQueryAsync();
}