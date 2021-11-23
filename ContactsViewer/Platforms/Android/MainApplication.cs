using Android.App;
using Android.Runtime;
using ContactsViewer.Platforms.Android.Handlers;

[assembly: UsesPermission(Android.Manifest.Permission.ReadContacts)]
[assembly: UsesPermission(Android.Manifest.Permission.ReadExternalStorage)]
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]

namespace ContactsViewer.Platforms.Android;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram
        .CreateMauiAppBuilder()
        .ConfigureMauiHandlers(delegate (IMauiHandlersCollection handlers)
        {
            var descriptorToRemove = handlers.FirstOrDefault(d => d.ServiceType == typeof(IBlazorWebView));
            handlers.Remove(descriptorToRemove);

            handlers.AddHandler<IBlazorWebView, CustomFilesBlazorWebViewHandler>();
        })
        .Build();
}