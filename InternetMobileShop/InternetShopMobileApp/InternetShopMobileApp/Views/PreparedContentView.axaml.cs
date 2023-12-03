using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using InternetShopMobileApp.ViewModels;
using ReactiveUI;

namespace InternetShopMobileApp.Views
{
    public partial class PreparedContentView : ReactiveUserControl<PreparedContentViewModel>
    {
        public PreparedContentView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
