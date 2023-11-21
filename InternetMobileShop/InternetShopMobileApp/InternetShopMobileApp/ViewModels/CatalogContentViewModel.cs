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

        public CatalogContentViewModel(IScreen screen) 
        { 
            HostScreen = screen;
            GoToProductPage = ReactiveCommand.Create(RunTheThing);

            Products = new ObservableCollection<ProductData>();
            LoadProducts().ConfigureAwait(false);
        }

        private IRoutableViewModel RunTheThing()
        {
            HostScreen.Router.Navigate.Execute(new ProductContentViewModel(HostScreen));
            return null;
        }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToProductPage { get; }
    }
}
