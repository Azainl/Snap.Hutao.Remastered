// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Service.Navigation;

public interface INavigationCompletionSource
{
    [SuppressMessage("", "SH003")]
    Task WaitForCompletionAsync();
}