using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using InternetShopMobileApp.ViewModels;
using ReactiveUI;

namespace InternetShopMobileApp.Views
{
    public partial class CatalogContentView : ReactiveUserControl<CatalogContentViewModel>
    {
        public CatalogContentView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
