using Microsoft.Extensions.FileProviders;

namespace ContactsViewer.App;

public partial class MainPage
{
    public MainPage()
    {
        InitializeComponent();
    }
}

public class ContactsViewerBlazorWebView : BlazorWebView
{
#if Android
    public override IFileProvider CreateFileProvider(string contentRootDir)
    {
        return new Platforms.Android.Implementations.ContactsViewFileProvider(contentRootDir);
    }
#endif
}
