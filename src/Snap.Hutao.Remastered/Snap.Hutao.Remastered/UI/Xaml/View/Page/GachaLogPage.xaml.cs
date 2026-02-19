// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.UI.Xaml.Control;
using Snap.Hutao.Remastered.ViewModel.GachaLog;
using System.Runtime.CompilerServices;

namespace Snap.Hutao.Remastered.UI.Xaml.View.Page;

public sealed partial class GachaLogPage : ScopedPage
{
    public GachaLogPage()
    {
        InitializeComponent();
    }

    protected override void LoadingOverride()
    {
        InitializeDataContext<GachaLogViewModel>();
        GachaLogViewModel viewModel = Unsafe.As<GachaLogViewModel>(DataContext);
        viewModel.gachaLogPage = this;
        viewModel.pivot = Pivot;
        viewModel.pivotAvatar = PivotAvatar;
        viewModel.pivotCountdown = PivotCountdown;
        viewModel.pivotHistory = PivotHistory;
        viewModel.pivotOverview = PivotOverview;
        viewModel.pivotStatistics = PivotStatistics;
        viewModel.pivotWeapon = PivotWeapon;
    }
}