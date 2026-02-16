// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Model.Metadata.Abstraction;

public interface IItemConvertible
{
    TItem ToItem<TItem>()
        where TItem : Model.Item, new();
}