using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using InternetShopMobileApp.ViewModels;
using ReactiveUI;

namespace InternetShopMobileApp.Views
{
    public partial class OrderManagementContentView : ReactiveUserControl<OrderManagementContentViewModel>
    {
        public OrderManagementContentView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
