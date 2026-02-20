// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml.Controls;

namespace Snap.Hutao.Remastered.UI.Xaml.View.Specialized;

[DependencyProperty<bool>("ShowUpPull", DefaultValue = true, NotNull = true)]
[DependencyProperty<bool>("ShowOrange", DefaultValue = true, NotNull = true)]
public sealed partial class StatisticsCard : UserControl
{
    public StatisticsCard()
    {
        InitializeComponent();
    }
}
