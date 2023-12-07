using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
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

    private bool _isSearchVisible;
    public bool IsSearchVisible
    {
        get => _isSearchVisible;
        set => this.RaiseAndSetIfChanged(ref _isSearchVisible, value);
    }

    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState router = new RoutingState();
    public RoutingState Router
    {
        get
        {
            //UpdateSearchVisibility(router.GetCurrentViewModel());
            return router;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref router, value);
        }
    }

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
            () => Router.Navigate.Execute(new CatalogContentViewModel(this, "." + SelectedKeyword)),
            canNavigate
        );

        Router.NavigationStack.CollectionChanged += (sender, e) =>
        {
            UpdateSearchVisibility();
        };

        Router.Navigate.Execute(new MainContentViewModel(this));
    }

    private void UpdateSearchVisibility()
    {
        var currentViewModel = Router.NavigationStack.LastOrDefault();
        // Здесь вы можете установить IsSearchVisible в true или false в зависимости от типа текущего ViewModel
        IsSearchVisible = currentViewModel is CatalogContentViewModel || currentViewModel is MainContentViewModel;
    }
}
