﻿using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace ContactsViewer.App.Platforms.Android.Implementations;

public class ContactsViewFileProvider : IFileProvider
{
    private readonly IFileProvider _fileProvider;

    public ContactsViewFileProvider(string contentRootDir)
    {
        var androidMauiAssetFileProviderType = typeof(BlazorWebView).Assembly.GetType("Microsoft.AspNetCore.Components.WebView.Maui.AndroidMauiAssetFileProvider");

        _fileProvider = (IFileProvider)Activator.CreateInstance(androidMauiAssetFileProviderType, new object[] { MauiApplication.Current.Assets, contentRootDir });
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        return _fileProvider.GetDirectoryContents(subpath);
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        if (subpath.EndsWith(".contact"))
        {
            subpath = $"content://com.android.contacts/display_photo/{subpath.Replace(".contact", string.Empty)}";

            return new ContactFileInfo(subpath);
        }

        return _fileProvider.GetFileInfo(subpath);
    }

    public IChangeToken Watch(string filter)
    {
        return _fileProvider.Watch(filter);
    }
}

public class ContactFileInfo : IFileInfo
{
    private readonly string _path;

    public ContactFileInfo(string path)
    {
        _path = path;
    }

    public bool Exists => true;

    public long Length => 0;

    public string PhysicalPath => "contact.png";

    public string Name => "contact.png";

    public DateTimeOffset LastModified => DateTimeOffset.UtcNow;

    public bool IsDirectory => false;

    public Stream CreateReadStream()
    {
        try
        {
            return MauiApplication.Current.ContentResolver.OpenInputStream(global::Android.Net.Uri.Parse(_path));
        }
        catch (Java.IO.FileNotFoundException)
        {
            return new MemoryStream();
        }
    }
}
