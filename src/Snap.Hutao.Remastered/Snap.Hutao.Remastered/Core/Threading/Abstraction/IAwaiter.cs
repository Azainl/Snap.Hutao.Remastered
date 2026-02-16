// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Runtime.CompilerServices;

namespace Snap.Hutao.Remastered.Core.Threading.Abstraction;

public interface IAwaiter : INotifyCompletion
{
    bool IsCompleted { get; }

    void GetResult();
}

public interface IAwaiter<out TResult> : INotifyCompletion
{
    bool IsCompleted { get; }

    TResult GetResult();
}