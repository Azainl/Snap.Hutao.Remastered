// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Web.Endpoint.Hutao;

[Service(ServiceLifetime.Singleton, typeof(IHutaoEndpoints), Key = HutaoEndpointsKind.Release)]
internal sealed class HutaoEndpointsForRelease : IHutaoEndpoints
{
    string IHomaRootAccess.Root { get => "https://homa.snap.hutaorp.org"; }

    string IInfrastructureRootAccess.Root { get => "https://api.snap.hutaorp.org"; }

    string IInfrastructureRawRootAccess.RawRoot { get => "https://api.snap.hutaorp.org"; }
}