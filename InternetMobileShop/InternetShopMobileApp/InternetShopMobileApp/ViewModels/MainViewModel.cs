using ReactiveUI;

namespace InternetShopMobileApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!!!!";

    private object _dynamicContent;
    public object DynamicContentContent
    {
        get { return _dynamicContent; }
        set { this.RaiseAndSetIfChanged(ref _dynamicContent, value); }
    }


    public MainViewModel()
    {
        DynamicContentContent = new MainContentView();
    }
}
