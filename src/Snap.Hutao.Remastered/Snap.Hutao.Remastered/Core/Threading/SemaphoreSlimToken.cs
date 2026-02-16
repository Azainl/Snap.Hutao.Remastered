// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Core.Threading;

public readonly struct SemaphoreSlimToken : IDisposable
{
    private readonly SemaphoreSlim semaphoreSlim;

    public SemaphoreSlimToken(SemaphoreSlim semaphoreSlim)
    {
        this.semaphoreSlim = semaphoreSlim;
    }

    public void Dispose()
    {
        semaphoreSlim.Release();
    }
}