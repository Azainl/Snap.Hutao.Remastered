// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Core.IO;
using System.IO;
using Windows.ApplicationModel;

namespace Snap.Hutao.Remastered.Core.Shell;

[Service(ServiceLifetime.Transient, typeof(IShellLinkInterop))]
public sealed class ShellLinkInterop : IShellLinkInterop
{
    public bool TryCreateDesktopShortcut()
    {
        string targetLogoPath = HutaoRuntime.GetDataDirectoryFile("ShellLinkLogo.ico");

        try
        {
            InstalledLocation.CopyFileFromApplicationUri("ms-appx:///Assets/Logo.ico", targetLogoPath);

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string target = Path.Combine(desktop, $"{SH.FormatAppNameAndVersion(HutaoRuntime.Version)}.lnk");

            FileSystem.CreateLink("hutao://", "", targetLogoPath, target);

            return true;
        }
        catch
        {
            return false;
        }
    }
}