using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using InternetShopMobileApp.ViewModels;
using ReactiveUI;

namespace InternetShopMobileApp.Views
{
    public partial class ProfileContentView : ReactiveUserControl<ProfileContentViewModel>
    {
        public ProfileContentView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}
