using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace ContactsViewer.App;

public static class MauiProgram
{
    public static MauiAppBuilder CreateMauiAppBuilder()
    {
        var builder = MauiApp.CreateBuilder();

        var assembly = typeof(MauiProgram).GetTypeInfo().Assembly;

        builder
                .UseMauiApp<App>()
                .Configuration.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false, false);

        var services = builder.Services;

        services.AddSingleton(scope => new HttpClient { BaseAddress = new Uri(scope.GetService<IConfiguration>().GetValue<string>("ApiServerAddress")) });
        services.AddContactsViewerSharedServices();
        services.AddContactsViewerServices();
        services.AddMauiBlazorWebView();

#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif

        return builder;
    }
}