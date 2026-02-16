// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Service.BackgroundImage;

public interface IBackgroundImageService
{
    ValueTask<ValueResult<bool, BackgroundImage?>> GetNextBackgroundImageAsync(BackgroundImage? previous, CancellationToken token = default);
}