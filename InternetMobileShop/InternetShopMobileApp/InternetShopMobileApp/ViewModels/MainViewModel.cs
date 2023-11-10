using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;

namespace InternetShopMobileApp.ViewModels;

public class MainViewModel : ReactiveObject, IScreen
{
    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState Router { get; } = new RoutingState();

    // The command that navigates a user to first view model.

    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToMain { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToProfile { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToCatalog { get; }

    public MainViewModel()
    {

        // Manage the routing state. Use the Router.Navigate.Execute
        // command to navigate to different view models. 
        //
        // Note, that the Navigate.Execute method accepts an instance 
        // of a view model, this allows you to pass parameters to 
        // your view models, or to reuse existing view models.
        //

        NavigateToMain = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new MainContentViewModel(this))
        );
        NavigateToProfile = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new ProfileContentViewModel(this))
        );
        NavigateToCatalog = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new CatalogContentViewModel(this))
        );

        Router.Navigate.Execute(new MainContentViewModel(this));
    }
}
