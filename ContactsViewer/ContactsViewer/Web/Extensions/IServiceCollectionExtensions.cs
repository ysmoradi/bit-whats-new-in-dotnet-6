namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddContactsViewerServices(this IServiceCollection services)
    {
        services.AddScoped<IToastService, ToastService>();


#if Android
        services.AddSingleton<IContactsService, ContactsViewer.App.Platforms.Android.Implementations.ContactsService>();
#elif iOS
        services.AddSingleton<IContactsService, ContactsViewer.App.Platforms.iOS.Implementations.ContactsService>();
#elif Windows
        services.AddSingleton<IContactsService, ContactsViewer.App.Platforms.Windows.Implementations.ContactsService>();
#elif Mac
        services.AddSingleton<IContactsService, ContactsViewer.App.Platforms.MacCatalyst.Implementations.ContactsService>();
#else
        services.AddSingleton<IContactsService, ContactsViewer.App.Platforms.Web.Implementations.ContactsService>();
#endif

        return services;
    }
}
