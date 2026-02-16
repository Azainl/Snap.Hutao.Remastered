using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Snap.Hutao.Remastered.API.Model;
using Snap.Hutao.Remastered.UI.Text;
using Snap.Hutao.Remastered.UI.Xaml;
using Snap.Hutao.Remastered.UI.Xaml.Control;
using System.IO;
using System.Reflection;

namespace Snap.Hutao.Remastered.API;

public partial class PluginScopedPage : ScopedPage
{
    public HutaoPlugin Plugin { get; private set; }
    protected PluginScopedPage(HutaoPlugin plugin)
    {
        Plugin = plugin;
    }

    protected void LoadXamlManually(string path)
    {
        try
        {
            Assembly assembly = GetType().Assembly;
            string resourceName = $"{Plugin.Manifest.Id}.{path}";

            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    string xamlFilePath = Path.Combine(Path.GetDirectoryName(assembly.Location)!, path);
                    if (File.Exists(xamlFilePath))
                    {
                        string xaml = File.ReadAllText(xamlFilePath);
                        LoadXamlFromString(xaml, path);
                    }
                    else
                    {
                        CreateFallbackUI(path);
                    }
                    return;
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string xaml = reader.ReadToEnd();
                    LoadXamlFromString(xaml, path);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading XAML: {ex.Message}");
            CreateFallbackUI(path);
        }
    }

    protected void LoadXamlFromString(string xaml, string path)
    {
        try
        {
            UIElement? uiElement = XamlReader.Load(xaml) as UIElement;
            if (uiElement != null)
            {
                this.Content = uiElement;
            }
            else
            {
                CreateFallbackUI(path);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing XAML: {ex.Message}");
            CreateFallbackUI(path);
        }
    }

    protected void CreateFallbackUI(string path)
    {
        Grid grid = new Grid();

        StackPanel panel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 20
        };

        TextBlock title = new TextBlock
        {
            Text = $"{Plugin.Manifest.Name}.{path}",
            FontSize = 24,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        panel.Children.Add(title);

        TextBlock message = new TextBlock
        {
            Text = "XAML loading failed.",
            HorizontalAlignment = HorizontalAlignment.Center
        };
        panel.Children.Add(message);

        grid.Children.Add(panel);
        this.Content = grid;
    }

    protected TViewModel LoadViewModel<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel.Abstraction.ViewModel
    {
        using (viewModel.CriticalSection.Enter())
        {
            viewModel.Resurrect();
            viewModel.CancellationToken = CancellationToken;
            viewModel.DeferContentLoader = new DeferContentLoader(this);

            DataContext = viewModel;
        }

        return viewModel;
    }
}
