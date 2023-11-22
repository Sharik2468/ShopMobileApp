using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;

namespace InternetShopMobileApp.ViewModels;

public class MainViewModel : ReactiveObject, IScreen
{

    private string _selectedKeyword;
    public string SelectedKeyword
    {
        get => _selectedKeyword;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedKeyword, value);
        }
    }

    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState Router { get; } = new RoutingState();

    // The command that navigates a user to first view model.

    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToMain { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToBasket { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToProfile { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToCatalog { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToSearchCatalog { get; }

    public MainViewModel()
    {

        // Manage the routing state. Use the Router.Navigate.Execute
        // command to navigate to different view models. 
        //
        // Note, that the Navigate.Execute method accepts an instance 
        // of a view model, this allows you to pass parameters to 
        // your view models, or to reuse existing view models.
        //

        var canNavigate = this.WhenAnyValue(x => x.SelectedKeyword)
                          .Select(keyword => !string.IsNullOrEmpty(keyword));

        NavigateToMain = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new MainContentViewModel(this))
        );
        NavigateToBasket = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new BasketContentViewModel(this))
        );
        NavigateToProfile = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new ProfileContentViewModel(this))
        );
        NavigateToCatalog = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new CatalogContentViewModel(this))
        );
        NavigateToSearchCatalog = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new CatalogContentViewModel(this, SelectedKeyword)),
            canNavigate
        );

        Router.Navigate.Execute(new MainContentViewModel(this));
    }
}
