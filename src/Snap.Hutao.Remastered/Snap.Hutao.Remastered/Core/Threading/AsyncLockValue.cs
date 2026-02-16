// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Core.Threading;

public sealed class AsyncLockValue<T>
    where T : class
{
    public AsyncLock Lock { get; } = new();

    public T? Value { get; set; }

    public Task<AsyncLock.Releaser> LockAsync()
    {
        return Lock.LockAsync();
    }
}