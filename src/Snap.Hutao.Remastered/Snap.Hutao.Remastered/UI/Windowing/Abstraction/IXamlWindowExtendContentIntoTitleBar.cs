// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using System.Collections.Immutable;

namespace Snap.Hutao.Remastered.UI.Windowing.Abstraction;

public interface IXamlWindowExtendContentIntoTitleBar
{
    FrameworkElement TitleBarCaptionAccess { get; }

    ImmutableArray<FrameworkElement> TitleBarPassthrough { get; }
}