using InternetShopMobileApp.DTOs;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using InternetShopMobileApp.Converters;
using Newtonsoft.Json;
using System.Net.Http;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Windows.Input;
using System.Globalization;
using System.Diagnostics.Metrics;

namespace InternetShopMobileApp.ViewModels
{
    public class MainContentViewModel : ReactiveObject, IRoutableViewModel
    {
        private ProductData _selectedProduct;
        public ProductData SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedProduct, value);
                HostScreen.Router.Navigate.Execute(new ProductContentViewModel(HostScreen, SelectedProduct));
            }
        }

        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ObservableCollection<ProductData> Products { get; set; }

        public async void LoadProducts()
        {
            HttpClient client = HttpClientInstance.Client;
            try
            {
                var response = await client.GetAsync(URLHelper.APIURL + "/api/Product");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<ProductData>>(jsonResponse);
                    products.Reverse();
                    products.Take(10);
                    foreach (var product in products)
                    {
                        Products.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToBit { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToSmartphone { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToTele { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToComp { get; }
        public MainContentViewModel(IScreen screen)
        {
            HostScreen = screen;

            Products = new ObservableCollection<ProductData>();
            LoadProducts();

            NavigateToBit = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new CatalogContentViewModel(HostScreen, "Техника для кухни."))
        );
            NavigateToSmartphone = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new CatalogContentViewModel(HostScreen, "Планшеты."))
        );
            NavigateToTele = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new CatalogContentViewModel(HostScreen, "Телевизоры и аксессуары."))
        );
            NavigateToComp = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new CatalogContentViewModel(HostScreen, "Комплектующие для ПК."))
        );
        }

    }
}
