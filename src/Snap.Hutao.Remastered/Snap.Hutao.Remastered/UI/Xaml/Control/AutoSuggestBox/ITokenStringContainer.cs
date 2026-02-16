// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.UI.Xaml.Control.AutoSuggestBox;

public interface ITokenStringContainer
{
    string? Text { get; set; }

    bool IsLast { get; }
}