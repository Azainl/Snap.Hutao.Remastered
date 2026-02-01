#tool "nuget:?package=nuget.commandline&version=6.9.1"
#addin nuget:?package=Cake.Http&version=4.0.0

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

// Pre-define

var version = "version";

var repoDir = "repoDir";
var outputPath = "outputPath";

var pfxPath = "pfxPath";
var pw = "pw";

// Extensions

static ProcessArgumentBuilder AppendIf(this ProcessArgumentBuilder builder, string text, bool condition)
{
    return condition ? builder.Append(text) : builder;
}

// Properties

string solution
{
    get => System.IO.Path.Combine(repoDir, "src", "Snap.Hutao", "Snap.Hutao.sln");
}
string project
{
    get => System.IO.Path.Combine(repoDir, "src", "Snap.Hutao", "Snap.Hutao", "Snap.Hutao.csproj");
}
string binPath
{
    get => System.IO.Path.Combine(repoDir, "src", "Snap.Hutao", "Snap.Hutao", "bin", "x64", "Release", "net10.0-windows10.0.26100.0", "win-x64");
}
string manifest
{
    get => System.IO.Path.Combine(repoDir, "src", "Snap.Hutao", "Snap.Hutao", "Package.appxmanifest");
}

repoDir = System.Environment.CurrentDirectory;
outputPath = System.IO.Path.Combine(repoDir, "src", "output");

version = System.DateTime.Now.ToString("yyyy.M.d.") + ((int)((System.DateTime.Now - System.DateTime.Today).TotalSeconds / 86400 * 65535)).ToString();

Information($"Version: {version}");

// Windows SDK
var registry = new WindowsRegistry();
var winsdkRegistry = registry.LocalMachine.OpenKey(@"SOFTWARE\Microsoft\Windows Kits\Installed Roots");
var winsdkVersion = winsdkRegistry.GetSubKeyNames().MaxBy(key => int.Parse(key.Split(".")[2]));
var winsdkPath = (string)winsdkRegistry.GetValue("KitsRoot10");
var winsdkBinPath = System.IO.Path.Combine(winsdkPath, "bin", winsdkVersion, "x64");
Information($"Windows SDK: {winsdkPath}");

Task("Build")
    .IsDependentOn("Build binary package")
    .IsDependentOn("Copy files")
    .IsDependentOn("Remove unused files")
    .IsDependentOn("Build MSIX")

Task("NuGet Restore")
    .Does(() =>
{
    Information("Restoring packages...");

    var nugetConfig = System.IO.Path.Combine(repoDir, "NuGet.Config");
    DotNetRestore(project, new DotNetRestoreSettings
    {
        Verbosity = DotNetVerbosity.Detailed,
        Interactive = false,
        ConfigFile = nugetConfig
    });
});

Task("Build binary package")
    .IsDependentOn("NuGet Restore")
    .IsDependentOn("Generate AppxManifest")
    .Does(() =>
{
    Information("Building binary package...");

    var settings = new DotNetBuildSettings
    {
        Configuration = configuration
    };

    settings.MSBuildSettings = new DotNetMSBuildSettings
    {
        ArgumentCustomization = args => args.Append("/p:Platform=x64")
                                            .Append("/p:UapAppxPackageBuildMode=SideloadOnly")
                                            .Append("/p:AppxPackageSigningEnabled=false")
                                            .Append("/p:AppxBundle=Never")
                                            .Append("/p:AppxPackageOutput=" + outputPath)
    };

    DotNetBuild(project, settings);
});

Task("Copy files")
    .IsDependentOn("Build binary package")
    .Does(() =>
{
    Information("Copying assets...");
    CopyDirectory(
        System.IO.Path.Combine(repoDir, "src", "Snap.Hutao.Remastered", "Snap.Hutao.Remastered", "Assets"),
        System.IO.Path.Combine(binPath, "Assets")
    );

    Information("Copying resource...");
    CopyDirectory(
        System.IO.Path.Combine(repoDir, "src", "Snap.Hutao.Remastered", "Snap.Hutao.Remastered", "Resource"),
        System.IO.Path.Combine(binPath, "Resource")
    );
});

Task("Remove unused files")
    .IsDependentOn("Build binary package")
    .Does(() =>
{
    Information("Removing unused files...");

    Information("Removing xbf...");
    System.IO.File.Delete(System.IO.Path.Combine(binPath, "App.xbf"));

    Information("Removing appxrecipe...");
    System.IO.File.Delete(System.IO.Path.Combine(binPath, "Snap.Hutao.Remastered.build.appxrecipe"));
});

Task("Build MSIX")
    .IsDependentOn("Build binary package")
    .IsDependentOn("Copy files")
    .IsDependentOn("Remove unused files")
    .Does(() =>
{
    var arguments = "arguments";
    if (GitHubActions.IsRunningOnGitHubActions)
    {
        arguments = "pack /d " + binPath + " /p " + System.IO.Path.Combine(outputPath, $"Snap.Hutao.Remastered{version}.msix");
    }
    else if (AppVeyor.IsRunningOnAppVeyor)
    {
        arguments = "pack /d " + binPath + " /p " + System.IO.Path.Combine(outputPath, $"Snap.Hutao.Remastered-{version}.msix");
    }
    else
    {
        arguments = "pack /d " + binPath + " /p " + System.IO.Path.Combine(outputPath, $"Snap.Hutao.Remastered.Local-{version}.msix");
    }

    var makeappxPath = System.IO.Path.Combine(winsdkBinPath, "makeappx.exe");

    var p = StartProcess(
        makeappxPath,
        new ProcessSettings
        {
            Arguments = arguments
        }
    );
    if (p != 0)
    {
        throw new InvalidOperationException("Build MSIX failed with exit code " + p);
    }
});

RunTarget(target);
