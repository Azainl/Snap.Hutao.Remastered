// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Remastered.API.Model;
using Snap.Hutao.Remastered.Core;
using Snap.Hutao.Remastered.Core.IO;
using Snap.Hutao.Remastered.Core.Logging;
using Snap.Hutao.Remastered.Factory.ContentDialog;
using Snap.Hutao.Remastered.Factory.Picker;
using Snap.Hutao.Remastered.Service.Navigation;
using Snap.Hutao.Remastered.Service.Notification;
using Snap.Hutao.Remastered.Service.Plugin;
using Snap.Hutao.Remastered.UI.Xaml.View.Page;
using System.Collections.ObjectModel;
using System.IO;

namespace Snap.Hutao.Remastered.ViewModel.Plugin;

[BindableCustomPropertyProvider]
[Service(ServiceLifetime.Scoped)]
public sealed partial class PluginViewModel : Abstraction.ViewModel
{
    private readonly IPluginService pluginService;
    private readonly IContentDialogFactory contentDialogFactory;
    private readonly ITaskContext taskContext;
    private readonly IFileSystemPickerInteraction fileSystem;
    private readonly IMessenger messenger;
    private readonly INavigationService navigationService;

    [GeneratedConstructor]
    public partial PluginViewModel(IServiceProvider serviceProvider);

    [ObservableProperty]
    public partial ObservableCollection<HutaoPlugin> Plugins { get; set; } = new();

    [ObservableProperty]
    public partial HutaoPlugin? SelectedPlugin { get; set; }

    protected override async ValueTask<bool> LoadOverrideAsync(CancellationToken token)
    {
        await taskContext.SwitchToMainThreadAsync();

        IEnumerable<HutaoPlugin> pluginList = pluginService.GetAllPlugins();
        Plugins = new ObservableCollection<HutaoPlugin>(pluginList);
        
        return true;
    }

    [Command("InstallPluginCommand")]
    private async Task InstallPluginAsync()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Install plugin", "PluginViewModel.Command"));

        try
        {

            ValueResult<bool, ValueFile> file = fileSystem.PickFile("选择插件文件", "HutaoPlugin", "*.hutao");
            if (!file.IsOk)
            {
                return;
            }

            string path = file.Value.ToString();
            bool result = await pluginService.InstallPluginAsync(path);
            
            if (result)
            {
                await LoadCommand.ExecuteAsync(null);
            }
        }
        catch (Exception ex)
        {
            messenger.Send(InfoBarMessage.Error("安装插件失败", ex));
        }
    }

    [Command("EnablePluginCommand")]
    private async Task EnablePluginAsync(HutaoPlugin? plugin)
    {
        if (plugin == null) return;

        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Enable plugin", "PluginViewModel.Command", new Dictionary<string, string>
        {
            { "PluginId", plugin.Manifest.Id },
            { "PluginName", plugin.Manifest.Name },
        }));

        if (plugin == null)
        {
            return;
        }

        bool result = await pluginService.EnablePluginAsync(plugin);
        if (result)
        {
            await LoadCommand.ExecuteAsync(null);
        }
    }

    [Command("DisablePluginCommand")]
    private async Task DisablePluginAsync(HutaoPlugin? plugin)
    {
        if (plugin == null) return;

        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Disable plugin", "PluginViewModel.Command", new Dictionary<string, string>
        {
            { "PluginId", plugin.Manifest.Id },
            { "PluginName", plugin.Manifest.Name },
        }));

        if (plugin == null)
        {
            return;
        }

        bool result = await pluginService.DisablePluginAsync(plugin);
        if (result)
        {
            await LoadCommand.ExecuteAsync(null);
        }
    }

    [Command("UninstallPluginCommand")]
    private async Task UninstallPluginAsync(HutaoPlugin? plugin)
    {
        if (plugin == null) return;

        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Uninstall plugin", "PluginViewModel.Command", new Dictionary<string, string>
        {
            { "PluginId", plugin.Manifest.Id },
            { "PluginName", plugin.Manifest.Name },
        }));

        if (plugin == null)
        {
            return;
        }

        ContentDialogResult result = await contentDialogFactory
            .CreateForConfirmCancelAsync(
                $"卸载插件 {plugin.Manifest.Name}",
                $"确定要卸载插件 {plugin.Manifest.Name} 吗？此操作不可恢复。")
            .ConfigureAwait(false);

        if (result is ContentDialogResult.Primary)
        {
            await pluginService.UninstallPlugin(plugin);
            await LoadCommand.ExecuteAsync(null);
        }
    }

    [Command("RefreshPluginsCommand")]
    private async Task RefreshPluginsAsync()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Refresh plugins", "PluginViewModel.Command"));

        await LoadCommand.ExecuteAsync(null);
    }

    [Command("OpenPluginDirectoryCommand")]
    private void OpenPluginDirectory()
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Open plugin directory", "PluginViewModel.Command"));

        string pluginsDirectory = Path.Combine(HutaoRuntime.DataDirectory, "Plugins");
        if (!Directory.Exists(pluginsDirectory))
        {
            Directory.CreateDirectory(pluginsDirectory);
        }
        
        System.Diagnostics.Process.Start("explorer.exe", pluginsDirectory);
    }

    [Command("OpenPluginSettingsCommand")]
    private async Task OpenPluginSettingsAsync(HutaoPlugin? plugin)
    {
        if (plugin == null) return;

        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateUI("Open plugin settings", "PluginViewModel.Command", new Dictionary<string, string>
        {
            { "PluginId", plugin.Manifest.Id },
            { "PluginName", plugin.Manifest.Name },
        }));

        await navigationService.NavigateAsync<PluginSettingPage>(new NavigationExtraData(plugin.Manifest.Id));
    }
}
