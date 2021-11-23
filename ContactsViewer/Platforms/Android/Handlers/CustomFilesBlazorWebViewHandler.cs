using Android.Webkit;

namespace ContactsViewer.Platforms.Android.Handlers
{
    public class CustomFilesBlazorWebViewHandler : BlazorWebViewHandler
    {
        protected override WebChromeClient GetWebChromeClient()
        {
            var webChromeClient = base.GetWebChromeClient();
            return webChromeClient;
        }

        protected override WebViewClient GetWebViewClient()
        {
            var webViewClient = base.GetWebViewClient();

            return webViewClient;
        }

        protected override WebView CreateNativeView()
        {
            var webView = base.CreateNativeView();

            webView.Settings.AllowContentAccess =
                webView.Settings.AllowFileAccess =
                webView.Settings.AllowFileAccessFromFileURLs =
                webView.Settings.AllowUniversalAccessFromFileURLs =
                true;

            return webView;
        }
    }
}
