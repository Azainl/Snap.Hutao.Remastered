// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Core.ExceptionService;

namespace Snap.Hutao.Remastered.Core.Caching;

public class publicImageCacheException : Exception, IpublicException
{
    private publicImageCacheException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    private publicImageCacheException(string message)
        : base(message)
    {
    }

    public static publicImageCacheException Throw(string message, Exception innerException)
    {
        throw new publicImageCacheException(message, innerException);
    }

    public static publicImageCacheException Throw(string message)
    {
        throw new publicImageCacheException(message);
    }
}