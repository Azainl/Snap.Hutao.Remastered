// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Core.DependencyInjection.Annotation.HttpClient;
using Snap.Hutao.Remastered.Model.Entity;
using Snap.Hutao.Remastered.Web.Endpoint.Hoyolab;
using Snap.Hutao.Remastered.Web.Request.Builder;
using Snap.Hutao.Remastered.Web.Request.Builder.Abstraction;
using Snap.Hutao.Remastered.Web.Response;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Snap.Hutao.Remastered.Web.Hoyolab.Passport;

[HttpClient(HttpClientConfiguration.XRpc6)]
public sealed partial class HoyoPlayPassportClientOversea : IHoyoPlayPassportClient
{
    private readonly IHttpRequestMessageBuilderFactory httpRequestMessageBuilderFactory;
    [FromKeyed(ApiEndpointsKind.Oversea)]
    private readonly IApiEndpoints apiEndpoints;
    private readonly HttpClient httpClient;

    [GeneratedConstructor]
    public partial HoyoPlayPassportClientOversea(IServiceProvider serviceProvider, HttpClient httpClient);

    public async ValueTask<Response<AuthTicketWrapper>> CreateAuthTicketAsync(User user, CancellationToken token = default)
    {
        string? sToken = user.SToken?.GetValueOrDefault(Cookie.STOKEN);
        ArgumentException.ThrowIfNullOrEmpty(sToken);
        ArgumentException.ThrowIfNullOrEmpty(user.Mid);

        AuthTicketRequestOversea data = new()
        {
            BizName = "hk4e_global",
            Mid = user.Mid,
            SToken = sToken,
        };

        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(apiEndpoints.AccountCreateAuthTicketByGameBiz())
            .PostJson(data);

        Response<AuthTicketWrapper>? resp = await builder
            .SendAsync<Response<AuthTicketWrapper>>(httpClient, token)
            .ConfigureAwait(false);

        return Response.Response.DefaultIfNull(resp);
    }

    public ValueTask<Response<QrLogin>> CreateQrLoginAsync(CancellationToken token = default)
    {
        return ValueTask.FromException<Response<QrLogin>>(new NotSupportedException());
    }

    public ValueTask<Response<QrLoginResult>> QueryQrLoginStatusAsync(string ticket, CancellationToken token = default)
    {
        return ValueTask.FromException<Response<QrLoginResult>>(new NotSupportedException());
    }

    public ValueTask<(string? Aigis, string? Risk, Response<LoginResult> Response)> LoginByPasswordAsync(IPassportPasswordProvider provider, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(provider.Account);
        ArgumentNullException.ThrowIfNull(provider.Password);

        return LoginByPasswordAsync(provider.Account, provider.Password, provider.Aigis, provider.Verify, token);
    }

    public async ValueTask<(string? Aigis, string? Risk, Response<LoginResult> Response)> LoginByPasswordAsync(string account, string password, string? aigis, string? verify, CancellationToken token = default)
    {
        Dictionary<string, string> data = new()
        {
            ["account"] = Encrypt(account),
            ["password"] = Encrypt(password),
        };

        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(apiEndpoints.AccountLoginByPassword())
            .PostJson(data);

        if (!string.IsNullOrEmpty(aigis))
        {
            builder.SetXrpcAigis(aigis);
        }

        if (!string.IsNullOrEmpty(verify))
        {
            builder.SetXrpcVerify(verify);
        }

        (HttpResponseHeaders? headers, Response<LoginResult>? resp) = await builder
            .SendAsync<Response<LoginResult>>(httpClient, token)
            .ConfigureAwait(false);

        string? rpcAigis = headers?.GetValuesOrDefault("X-Rpc-Aigis")?.SingleOrDefault();
        string? rpcVerify = headers?.GetValuesOrDefault("X-Rpc-Verify")?.SingleOrDefault();
        return (rpcAigis, rpcVerify, Response.Response.DefaultIfNull(resp));
    }

    public async ValueTask<(string? Risk, Response<LoginResult> Response)> LoginByThirdPartyAsync(ThirdPartyToken thirdPartyToken, CancellationToken token = default)
    {
        HttpRequestMessageBuilder builder = httpRequestMessageBuilderFactory.Create()
            .SetRequestUri(apiEndpoints.AccountLoginByThirdParty())
            .PostJson(thirdPartyToken);

        (HttpResponseHeaders? headers, Response<LoginResult>? resp) = await builder
            .SendAsync<Response<LoginResult>>(httpClient, token)
            .ConfigureAwait(false);

        string? rpcVerify = headers?.GetValuesOrDefault("X-Rpc-Verify")?.SingleOrDefault();
        return (rpcVerify, Response.Response.DefaultIfNull(resp));
    }

    private static string Encrypt(string source)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportFromPem($"""
                -----BEGIN RSA PUBLIC KEY-----
                MIIBCgKCAQEAtjUS4JLsfCVbueTReY1E/kFBzoCQZtbSGW/BatU9BaZbgd1iIHKb
                CQhs0Uf3POcavhqW2/UVTxBlhi3cfzRVBbd61FZk0Xt5EI+8SvGxVR176yobMvZt
                7JcpommpY4RvfItiiag5GplLS9jrkOPlKsQiIAZaawgxL1HVpf6cPkHzfZCuzlDO
                agntlYgP8wkZI+K6E63AHFRgfU7n0YO9jUPhSDPRXvGo5n/5B19L+fiPYbf++e8z
                ywZVtoJ5ztJ/+6Vp+N7H/C5PEJGy9ZKHWSvTvCs8Curg2ahB8xD1aPd1XEOmkMBf
                DVuD0wOHnn7I03seIOU7l0w8ojP3E+eG3QIDAQAB
                -----END RSA PUBLIC KEY-----
                """);
            return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(source), RSAEncryptionPadding.Pkcs1));
        }
    }
}