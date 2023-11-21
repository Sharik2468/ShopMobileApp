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
            HttpClient client = new HttpClient();
            try
            {
                //client.Timeout = TimeSpan.FromSeconds(30); // Например, 30 секунд

                var response = await client.GetAsync(URLHelper.APIURL + "/api/Product");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<ProductData>>(jsonResponse).Take(10);
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
            finally
            {
                client.Dispose();
            }
        }

        public MainContentViewModel(IScreen screen)
        {
            HostScreen = screen;

            Products = new ObservableCollection<ProductData>();
            LoadProducts();
        }

    }
}
