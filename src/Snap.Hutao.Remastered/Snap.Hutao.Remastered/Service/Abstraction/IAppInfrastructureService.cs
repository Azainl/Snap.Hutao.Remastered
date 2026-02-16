// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Service.Abstraction;

public interface IAppInfrastructureService : IAppService
{
    IServiceProvider ServiceProvider { get; }
}