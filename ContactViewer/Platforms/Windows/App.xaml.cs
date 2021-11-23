using Microsoft.UI.Xaml;
using Windows.ApplicationModel;

namespace ContactViewer.Platforms.Windows;

public partial class App
{
    public App()
    {
        InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        Microsoft.Maui.Essentials.Platform.OnLaunched(args);
    }
}
