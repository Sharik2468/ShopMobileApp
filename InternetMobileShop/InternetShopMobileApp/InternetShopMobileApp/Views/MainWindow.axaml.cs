using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using System.Net;

namespace InternetShopMobileApp.Views;

public partial class MainWindow : Window
{
    private static MainWindow instance;

    public static MainWindow getInstance()
    {
        if (instance == null)
            instance = new MainWindow();
        return instance;
    }
    public MainWindow()
    {
        instance = this;
        InitializeComponent();

    }

    public void UploadFile()
    {
        var files = StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "aaa",
            //You can add either custom or from the built-in file types. See "Defining custom file types" on how to create a custom one.
            FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
        });
    }
}
