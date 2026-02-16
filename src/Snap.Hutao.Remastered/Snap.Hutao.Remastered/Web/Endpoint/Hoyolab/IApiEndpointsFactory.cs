// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Web.Endpoint.Hoyolab;

public interface IApiEndpointsFactory
{
    IApiEndpoints Create(ApiEndpointsKind kind);
}