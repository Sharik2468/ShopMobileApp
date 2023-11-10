using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace InternetShopMobileApp.ViewModels
{
    public class CatalogContentViewModel : ReactiveObject, IRoutableViewModel
    {
        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public CatalogContentViewModel(IScreen screen) 
        { 
            HostScreen = screen;
            GoToProductPage = ReactiveCommand.Create(RunTheThing);
        }

        private IRoutableViewModel RunTheThing()
        {
            HostScreen.Router.Navigate.Execute(new ProductContentViewModel(HostScreen));
            return null;
        }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToProductPage { get; }
    }
}
