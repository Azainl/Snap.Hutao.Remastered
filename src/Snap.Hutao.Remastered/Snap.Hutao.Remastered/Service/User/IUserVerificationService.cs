// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Web.Hoyolab.Passport;

namespace Snap.Hutao.Remastered.Service.User;

public interface IUserVerificationService
{
    ValueTask<bool> TryVerifyAsync(IVerifyProvider provider, string? rawRisk, bool isOversea, CancellationToken token = default);
}