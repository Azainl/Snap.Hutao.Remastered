// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Core.Abstraction;
using System.Net.Http.Headers;

namespace Snap.Hutao.Remastered.Web.Request.Builder.Abstraction;

public interface IHttpHeadersBuilder<out T> : IBuilder
    where T : HttpHeaders
{
    T Headers { get; }
}