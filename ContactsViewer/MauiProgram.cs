﻿namespace ContactsViewer;

public static class MauiProgram
{
    public static MauiAppBuilder CreateMauiAppBuilder()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        var services = builder.Services;

        services.AddBlazorWebView();
        services.AddSingleton<ContactsService>();

        return builder;
    }
}