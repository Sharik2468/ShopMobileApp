using InternetShopMobileApp.DTOs;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using InternetShopMobileApp.Converters;

namespace InternetShopMobileApp.ViewModels
{
    public class MainContentViewModel : ReactiveObject, IRoutableViewModel
    {
        public ObservableCollection<ProductDTO> Products { get; }

        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public MainContentViewModel(IScreen screen)
        {
            HostScreen = screen;
            GoToProductPage = ReactiveCommand.Create(RunTheThing);

            // Initialize the Products collection with example data
            Products = new ObservableCollection<ProductDTO>
            {
                // Example data
                new ProductDTO { ImageSource = ImagePathConverter.LoadFromResource(new Uri("avares://InternetShopMobileApp/Resources/monitor.png")), Title = "Монитор 1", Price = "12 000р.", Availability = "В наличии: 4 шт." },
                new ProductDTO { ImageSource = ImagePathConverter.LoadFromResource(new Uri("avares://InternetShopMobileApp/Resources/monitor.png")), Title = "Монитор 1", Price = "12 000р.", Availability = "В наличии: 4 шт." },
                new ProductDTO { ImageSource = ImagePathConverter.LoadFromResource(new Uri("avares://InternetShopMobileApp/Resources/monitor.png")), Title = "Монитор 1", Price = "12 000р.", Availability = "В наличии: 4 шт." },
                // Add other products here
            };
        }

        private IRoutableViewModel RunTheThing()
        {
            HostScreen.Router.Navigate.Execute(new ProductContentViewModel(HostScreen));
            return null;
        }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToProductPage { get; }
    }
}
