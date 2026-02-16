// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Frozen;

namespace Snap.Hutao.Remastered.Core;

public static class WinRTAdaptive
{
    public static FrozenDictionary<TKey, TValue> ToFrozenDictionary<TKey, TValue>(KeyValuePair<TKey, TValue>[] source)
        where TKey : notnull
    {
        return source.ToFrozenDictionary();
    }

    public static T[] Array<T>(T[] source)
    {
        return source;
    }
}