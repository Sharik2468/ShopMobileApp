using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InternetShopMobileApp.DialogueWindows
{
    public partial class ErrorDialogContent : UserControl
    {
        public ErrorDialogContent()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            InteractiveContainer.CloseDialog();
        }
    }
}
