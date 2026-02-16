using Snap.Hutao.Remastered.API.Model;

namespace Snap.Hutao.Remastered.Service.Plugin;

public interface IPluginService
{
    Task<bool> InstallPluginAsync(string path);

    Task<bool> LoadPluginAsync(string id);

    Task<bool> EnablePluginAsync(HutaoPlugin plugin);

    Task<bool> DisablePluginAsync(HutaoPlugin plugin); 

    Task UninstallPlugin(string id);

    async Task UninstallPlugin(HutaoPlugin plugin)
    {
        await UninstallPlugin(plugin.Manifest.Id);
    }

    HutaoPlugin? GetPluginById(string id);

    IEnumerable<HutaoPlugin> GetAllPlugins();

    Task LoadAllPluginsAsync();

    string? GetPluginPath(string id);

    string GetPluginPath(HutaoPlugin plugin);

    string GetPluginsDirectory();

    IPluginSettingService GetSettingService();
}
