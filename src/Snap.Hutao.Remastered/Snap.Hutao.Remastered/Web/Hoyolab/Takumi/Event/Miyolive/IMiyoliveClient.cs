// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Web.Response;

namespace Snap.Hutao.Remastered.Web.Hoyolab.Takumi.Event.Miyolive;

public interface IMiyoliveClient
{
    ValueTask<Response<CodeListWrapper>> RefreshCodeAsync(string actId, CancellationToken token = default);
}