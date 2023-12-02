using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System;

namespace InternetShopMobileApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void ShowNotification(string Title, string Message)
    {
        SukiUI.MessageBox.MessageBox.Info(this, Title, Message);

    }
}
