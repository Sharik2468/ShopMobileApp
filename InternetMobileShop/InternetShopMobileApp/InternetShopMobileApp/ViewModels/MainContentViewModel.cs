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
using System.Threading.Tasks;

namespace InternetShopMobileApp.ViewModels
{
    public class MainContentViewModel : ReactiveObject, IRoutableViewModel
    {
        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ObservableCollection<ProductData> Products { get; set; }

        private const int PageSize = 5; // количество элементов на странице

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }

        private int _totalPages;
        public int TotalPages
        {
            get => _totalPages;
            set { this.RaiseAndSetIfChanged(ref _totalPages, value); }
        }

        public ObservableCollection<ProductData> CurrentPageProducts { get; } = new ObservableCollection<ProductData>();

        // Команды для навигации между страницами
        public ReactiveCommand<Unit, Unit> NextPageCommand { get; }
        public ReactiveCommand<Unit, Unit> PrevPageCommand { get; }

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


                    // Ограничиваем загрузку продуктов для текущей страницы
                    products.Reverse();
                    var filterProducts = products.Where(a => a.NumberInStock != 0).Take(10);
                    Products.Clear();
                    foreach (var product in filterProducts)
                    {
                        Products.Add(product);
                    }

                    TotalPages = (int)Math.Ceiling(Products.Count / (double)PageSize);
                    UpdateCurrentPageProducts();
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
        public ReactiveCommand<int, Unit> NavigateToProduct { get; }
        public MainContentViewModel(IScreen screen)
        {
            HostScreen = screen;

            Products = new ObservableCollection<ProductData>();
            LoadProducts();

            NavigateToProduct = ReactiveCommand.CreateFromTask<int>(AdditionalInfo);
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

            NextPageCommand = ReactiveCommand.Create(NextPage, this.WhenAnyValue(x => x.CurrentPage, x => x.TotalPages, (currentPage, totalPages) => currentPage < totalPages));
            PrevPageCommand = ReactiveCommand.Create(PrevPage, this.WhenAnyValue(x => x.CurrentPage, currentPage => currentPage > 1));

        }

        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                UpdateCurrentPageProducts();
            }
        }

        private void PrevPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdateCurrentPageProducts();
            }
        }

        private void UpdateCurrentPageProducts()
        {
            var startIndex = (CurrentPage - 1) * PageSize;
            CurrentPageProducts.Clear();
            foreach (var product in Products.Skip(startIndex).Take(PageSize))
            {
                CurrentPageProducts.Add(product);
            }
        }

        private async Task AdditionalInfo(int productCode)
        {
            HostScreen.Router.Navigate.Execute(new ProductContentViewModel(HostScreen, productCode));
        }

    }
}
