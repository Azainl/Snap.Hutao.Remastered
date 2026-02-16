using System.IO;
using System.IO.Compression;
using System.Reflection;
using Snap.Hutao.Remastered.API.Annotation;
using Snap.Hutao.Remastered.API.Model;
using Snap.Hutao.Remastered.Core;
using Snap.Hutao.Remastered.Core.Logging;
using Snap.Hutao.Remastered.Service.Notification;

namespace Snap.Hutao.Remastered.Service.Plugin;

[Service(ServiceLifetime.Singleton, typeof(IPluginService))]
public partial class PluginService : IPluginService
{
    private readonly List<HutaoPlugin> plugins = new();
    private readonly Dictionary<string, string> pluginPaths = new();

    private readonly ITaskContext taskContext;
    private readonly IMessenger messenger;
    private readonly IServiceProvider serviceProvider;

    [GeneratedConstructor]
    public partial PluginService(IServiceProvider serviceProvider);

    public string PluginsDirectory => Path.Combine(HutaoRuntime.DataDirectory, "Plugins");

    public IReadOnlyList<HutaoPlugin> Plugins => plugins.AsReadOnly();

    public async Task<bool> DisablePluginAsync(HutaoPlugin plugin)
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateInfo("Disabling plugin", category: "PluginService", data: new Dictionary<string, string>
        {
            { "PluginId", plugin.Manifest.Id },
            { "PluginName", plugin.Manifest.Name },
        }));

        try
        {
            string pluginId = plugin.Manifest.Id;

            string enabledPath = Path.Combine(PluginsDirectory, $"{pluginId}.hutao");
            string disabledPath = Path.Combine(PluginsDirectory, $"{pluginId}.hutaodisabled");

            if (!File.Exists(enabledPath))
            {
                messenger.Send(InfoBarMessage.Error(SH.FormatServicePluginNotFound(plugin.Manifest.Name)));
                return false;
            }

            File.Move(enabledPath, disabledPath);

            plugin.OnDisable();
            plugin.IsEnabled = false;

            messenger.Send(InfoBarMessage.Success(SH.FormatServicePluginDisableSuccess(plugin.Manifest.Name)));
            return true;
        }
        catch (Exception ex)
        {
            messenger.Send(InfoBarMessage.Error(SH.ServicePluginDisablingPluginFailed, ex));
            return false;
        }
    }

    public async Task<bool> EnablePluginAsync(HutaoPlugin plugin)
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateInfo("Enabling plugin", category: "PluginService", data: new Dictionary<string, string>
        {
            { "PluginId", plugin.Manifest.Id },
            { "PluginName", plugin.Manifest.Name },
        }));

        try
        {
            string pluginId = plugin.Manifest.Id;

            string enabledPath = Path.Combine(PluginsDirectory, $"{pluginId}.hutao");
            string disabledPath = Path.Combine(PluginsDirectory, $"{pluginId}.hutaodisabled");

            if (File.Exists(disabledPath))
            {
                File.Move(disabledPath, enabledPath);
            }

            plugin.OnEnable();
            plugin.IsEnabled = true;

            messenger.Send(InfoBarMessage.Success(SH.FormatServicePluginEnablingSuccess(plugin.Manifest.Name)));
            return true;
        }
        catch (Exception ex)
        {
            messenger.Send(InfoBarMessage.Error(SH.ServicePluginEnablingPluginFailed, ex));
            return false;
        }
    }

    public HutaoPlugin? GetPluginById(string id)
    {
        return plugins.FirstOrDefault(p => p.Manifest.Id == id);
    }

    public async Task<bool> InstallPluginAsync(string path)
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateInfo("Installing plugin", category: "PluginService", data: new Dictionary<string, string>
        {
            { "SourcePath", path },
        }));

        try
        {
            Directory.CreateDirectory(PluginsDirectory);

            if (!File.Exists(path))
            {
                messenger.Send(InfoBarMessage.Error(SH.ServicePluginInstallFileNotFound));
                return false;
            }

            string extension = Path.GetExtension(path);
            if (extension != ".hutao" && extension != ".hutaodisabled")
            {
                messenger.Send(InfoBarMessage.Error(SH.ServicePluginInstallInvalidFileType));
                return false;
            }

            using ZipArchive archive = await ZipFile.OpenReadAsync(path);
            
            ZipArchiveEntry? manifestEntry = archive.GetEntry("manifest.json");
            if (manifestEntry == null)
            {
                messenger.Send(InfoBarMessage.Error(SH.ServicePluginInstallManifestNotFound));
                return false;
            }

            using Stream manifestStream = manifestEntry.Open();
            PluginManifest? manifest = await JsonSerializer.DeserializeAsync<PluginManifest>(manifestStream).ConfigureAwait(false);
            
            if (manifest == null || string.IsNullOrEmpty(manifest.Id))
            {
                messenger.Send(InfoBarMessage.Error(SH.ServicePluginInstallInvalidManifest));
                return false;
            }

            string pluginId = manifest.Id;

            string targetFileName = extension == ".hutao" ? $"{pluginId}.hutao" : $"{pluginId}.hutaodisabled";
            string targetPath = Path.Combine(PluginsDirectory, targetFileName);
            
            if (File.Exists(targetPath))
            {
                messenger.Send(InfoBarMessage.Error(SH.FormatServicePluginInstallAlreadyExists(manifest.Name)));
                return false;
            }

            File.Copy(path, targetPath, true);

            if (extension == ".hutao")
            {
                await LoadPluginAsync(pluginId).ConfigureAwait(false);

                HutaoPlugin? loadedPlugin = GetPluginById(pluginId);
                if (loadedPlugin != null)
                {
                    loadedPlugin.OnInstall();
                }
            }

            messenger.Send(InfoBarMessage.Success(SH.FormatServicePluginInstallSuccess(manifest.Name)));
            return true;
        }
        catch (Exception ex)
        {
            messenger.Send(InfoBarMessage.Error(SH.ServicePluginInstallFailed, ex));
            return false;
        }
    }

    public async Task<bool> LoadPluginAsync(string id)
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateInfo("Loading plugin", category: "PluginService", data: new Dictionary<string, string>
        {
            { "PluginId", id },
        }));

        try
        {
            HutaoPlugin? loadedPlugin = GetPluginById(id);
            if (loadedPlugin != null)
            {
                return await EnablePluginAsync(loadedPlugin);
            }

            bool isEnabled = true;
            string pluginFilePath = Path.Combine(PluginsDirectory, $"{id}.hutao");
            if (!File.Exists(pluginFilePath))
            {
                isEnabled = false;
                pluginFilePath = Path.Combine(PluginsDirectory, $"{id}.hutaodisabled");
                if (!File.Exists(pluginFilePath))
                {
                    messenger.Send(InfoBarMessage.Error(SH.FormatServicePluginLoadDirectoryNotFound(id)));
                    return false;
                }
            }

            string tempDirectory = Path.Combine(Path.GetTempPath(), $"hutao_plugin_{id}_{Guid.NewGuid():N}");
            Directory.CreateDirectory(tempDirectory);

            try
            {
                using ZipArchive archive = ZipFile.OpenRead(pluginFilePath);
                
                ZipArchiveEntry? manifestEntry = archive.GetEntry("manifest.json");
                if (manifestEntry == null)
                {
                    messenger.Send(InfoBarMessage.Error(SH.FormatServicePluginLoadManifestNotFound(id)));
                    return false;
                }

                using Stream manifestStream = manifestEntry.Open();
                PluginManifest? manifest = await JsonSerializer.DeserializeAsync<PluginManifest>(manifestStream).ConfigureAwait(false);
                
                if (manifest == null)
                {
                    messenger.Send(InfoBarMessage.Error(SH.FormatServicePluginLoadInvalidManifest(id)));
                    return false;
                }

                archive.ExtractToDirectory(tempDirectory, true);

                string dllPath = Path.Combine(tempDirectory, $"{id}.dll");
                if (!File.Exists(dllPath))
                {
                    dllPath = Path.Combine(tempDirectory, "lib", $"{id}.dll");
                    if (!File.Exists(dllPath))
                    {
                        messenger.Send(InfoBarMessage.Error(SH.FormatServicePluginLoadDllNotFound(id)));
                        return false;
                    }
                }

                foreach (FileInfo libFile in new DirectoryInfo(Path.GetDirectoryName(dllPath)!).GetFiles("*.dll"))
                {
                    Assembly.LoadFrom(libFile.FullName);
                }

                Assembly assembly = Assembly.LoadFrom(dllPath);
                Type? pluginType = FindPluginMainType(assembly);

                if (pluginType == null)
                {
                    messenger.Send(InfoBarMessage.Error(SH.FormatServicePluginLoadMainTypeNotFound(manifest.Name)));
                    return false;
                }

                HutaoPlugin plugin = (HutaoPlugin)pluginType.GetConstructors()[0].Invoke(Array.Empty<object>());
                plugin.Manifest = manifest;
                
                string iconPath = Path.Combine(tempDirectory, "icon.png");
                if (File.Exists(iconPath))
                {
                    plugin.IconPath = iconPath;
                }

                plugin.OnLoad(new PluginContext(serviceProvider));

                plugins.Add(plugin);
                pluginPaths[id] = tempDirectory;

                if (isEnabled)
                {
                    await EnablePluginAsync(plugin);
                }

                messenger.Send(InfoBarMessage.Success(SH.FormatServicePluginLoadSuccess(manifest.Name)));
                return true;
            }
            catch (Exception)
            {
                try
                {
                    Directory.Delete(tempDirectory, true);
                }
                catch
                {

                }
                throw;
            }
        }
        catch (Exception ex)
        {
            messenger.Send(InfoBarMessage.Error(SH.ServicePluginLoadFailed, ex));
            return false;
        }
    }

    public async Task LoadAllPluginsAsync()
    {
        foreach (DirectoryInfo tempDir in new DirectoryInfo(Path.GetTempPath()).GetDirectories("hutao_plugin_*"))
        {
            try
            {
                tempDir.Delete(true);
            }
            catch
            {
            }
        }
        try
        {
            if (!Directory.Exists(PluginsDirectory))
            {
                return;
            }

            string[] enabledPluginFiles = Directory.GetFiles(PluginsDirectory, "*.hutao");
            string[] disabledPluinFiles = Directory.GetFiles(PluginsDirectory, "*.hutaodisabled");

            foreach (string pluginFile in enabledPluginFiles)
            {
                string pluginId = Path.GetFileNameWithoutExtension(pluginFile);
                await LoadPluginAsync(pluginId).ConfigureAwait(false);
            }

            foreach (string pluginFile in disabledPluinFiles)
            {
                string pluginId = Path.GetFileNameWithoutExtension(pluginFile);
                await LoadPluginAsync(pluginId).ConfigureAwait(false);
            }
        }
        catch
        {

        }
    }

    private async Task ParsePluginFilesAsync(string pluginDirectory, PluginManifest manifest)
    {
        try
        {
            string dllFileName = $"{manifest.Id}.dll";
            string dllPath = Path.Combine(pluginDirectory, dllFileName);

            string iconPath = Path.Combine(pluginDirectory, "icon.png");
            if (File.Exists(iconPath))
            {
                manifest.IconPath = iconPath;
            }

            string injectDirectory = Path.Combine(pluginDirectory, "inject");
        }
        catch
        {

        }
    }

    private Type? FindPluginMainType(Assembly assembly)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (type.GetCustomAttribute<PluginMainAttribute>() != null &&
                typeof(HutaoPlugin).IsAssignableFrom(type))
            {
                return type;
            }
        }

        return null;
    }

    public async Task UninstallPlugin(string id)
    {
        SentrySdk.AddBreadcrumb(BreadcrumbFactory.CreateInfo("Uninstalling plugin", category: "PluginService", data: new Dictionary<string, string>
        {
            { "PluginId", id },
        }));

        HutaoPlugin? loadedPlugin = GetPluginById(id);

        if (loadedPlugin != null && loadedPlugin.IsEnabled)
        {
            await DisablePluginAsync(loadedPlugin).ConfigureAwait(false);
        }

        try
        {
            loadedPlugin?.OnUninstall();
            File.Delete(Path.Combine(PluginsDirectory, $"{id}.hutao"));
            File.Delete(Path.Combine(PluginsDirectory, $"{id}.hutaodisabled"));

            if (loadedPlugin != null)
            {
                plugins.Remove(loadedPlugin);
            }
        }
        catch
        {
            messenger.Send(InfoBarMessage.Success(SH.FormatServicePluginDeleteFailed(id)));
        }

        messenger.Send(InfoBarMessage.Success(SH.FormatServicePluginDeleteSuccess(id)));
    }

    public IEnumerable<HutaoPlugin> GetAllPlugins()
    {
        return plugins.AsReadOnly();
    }

    public string? GetPluginPath(string id)
    {
        if (pluginPaths.TryGetValue(id, out string? path))
        {
            return path;
        }
        return null;
    }

    public string GetPluginPath(HutaoPlugin plugin)
    {
        return GetPluginPath(plugin.Manifest.Id)!;
    }

    public string GetPluginsDirectory()
    {
        return PluginsDirectory;
    }

    public IPluginSettingService GetSettingService()
    {
        return serviceProvider.GetService(typeof(IPluginSettingService)) as IPluginSettingService ?? throw new InvalidOperationException("PluginSettingService not found.");
    }
}
