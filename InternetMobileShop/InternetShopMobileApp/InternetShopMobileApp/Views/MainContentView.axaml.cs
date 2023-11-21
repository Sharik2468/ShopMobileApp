using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using InternetShopMobileApp.ViewModels;
using ReactiveUI;

namespace InternetShopMobileApp.Views
{
    public partial class MainContentView : ReactiveUserControl<MainContentViewModel>
    {
        public MainContentView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }

        private void Border_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
        }

        private void ListBox_Tapped(object? sender, Avalonia.Input.TappedEventArgs e)
        {
        }

        private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
        }
    }
}
