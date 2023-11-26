using Avalonia.Threading;
using InternetShopMobileApp.DTOs;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;

namespace InternetShopMobileApp.ViewModels
{
    public class CatalogContentViewModel : ReactiveObject, IRoutableViewModel
    {
        public ObservableCollection<ProductData> Products { get; set; }

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

        public async Task LoadProducts()
        {
            HttpClient client = new HttpClient();
            try
            {
                //client.Timeout = TimeSpan.FromSeconds(30); // Например, 30 секунд

                var response = await client.GetAsync(URLHelper.APIURL + "/api/Product");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var allProducts = JsonConvert.DeserializeObject<List<ProductData>>(jsonResponse);
                    allProducts.Reverse();
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        foreach (var product in allProducts)
                        {
                            Products.Add(product);
                        }
                    });

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

        public async Task LoadSelectedProducts(string keyword)
        {
            HttpClient client = new HttpClient();
            try
            {
                //client.Timeout = TimeSpan.FromSeconds(30); // Например, 30 секунд

                var response = await client.GetAsync(URLHelper.APIURL + "/api/Product/keywordSearch/" + keyword);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var allProducts = JsonConvert.DeserializeObject<List<ProductData>>(jsonResponse);
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        foreach (var product in allProducts)
                        {
                            Products.Add(product);
                        }
                    });

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

        public CatalogContentViewModel(IScreen screen, string keyword = null)
        {
            HostScreen = screen;
            GoToProductPage = ReactiveCommand.Create(RunTheThing);

            Products = new ObservableCollection<ProductData>();
            if (keyword == null)
                LoadProducts().ConfigureAwait(false);
            else
                LoadSelectedProducts(keyword).ConfigureAwait(false);
        }

        private IRoutableViewModel RunTheThing()
        {
            HostScreen.Router.Navigate.Execute(new ProductContentViewModel(HostScreen));
            return null;
        }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToProductPage { get; }
    }
}
