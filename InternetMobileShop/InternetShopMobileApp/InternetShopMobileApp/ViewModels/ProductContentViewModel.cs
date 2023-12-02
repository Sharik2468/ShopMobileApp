using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using InternetShopMobileApp.DTOs;
using InternetShopMobileApp.Views;
using Newtonsoft.Json;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetShopMobileApp.Services;
using InternetShopMobileApp.DialogueWindows;

namespace InternetShopMobileApp.ViewModels
{
    public class ProductContentViewModel : ReactiveObject, IRoutableViewModel
    {
        public ReactiveCommand<Unit, IRoutableViewModel> AddToBasket { get; }

        private ProductData _selectedProduct;
        public ProductData SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedProduct, value);
            }
        }

        public string AvailableStockText => $"В наличии: {SelectedProduct?.NumberInStock ?? 0} шт.";

        public string PriceText => $"{SelectedProduct?.MarketPriceProduct ?? 0} р.";

        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ProductContentViewModel(IScreen screen, ProductData productData = null)
        {
            HostScreen = screen;
            SelectedProduct = productData;

            // Создайте экземпляр команды и укажите делегат для выполнения
            AddToBasket = ReactiveCommand.CreateFromTask<Unit, IRoutableViewModel>(AddproductToBasket);
        }

        private async Task<IRoutableViewModel> AddproductToBasket(Unit _)
        {
            AccountService serviceAccount = new AccountService();
            BasketService serviceBasket = new BasketService();

            var resultAccount = await serviceAccount.IsAuthentificated();

            switch (resultAccount.Result)
            {
                case AccountOutput.SUCCESS:
                    bool isAuthenticated = resultAccount.UserData.isAuthenticated.ToObject<bool>();
                    string userRole = resultAccount.UserData.userRole.ToObject<string>();
                    // Предположим, у вас есть способ получения ID пользователя
                    var userID = resultAccount.UserData.userID.ToObject<int>();
                    if (userID != null && isAuthenticated && userRole == "admin")
                    {
                        InteractiveContainer.ShowDialog(new RolesErrorDialogContent());
                        break;
                    }

                    if (userID != null && isAuthenticated)
                    {
                        var resultOrderItems = await serviceBasket.AddProductToBasket(userID, SelectedProduct.ProductCode);
                        InteractiveContainer.ShowToast(new TextBlock() { Text = "Товар добавлен в корзину!", Margin = new Thickness(15, 8) }, 5);
                        if (resultOrderItems.Result == BasketOutput.SUCCESS)
                        {
                            // Успешное добавление товара в корзину
                            InteractiveContainer.ShowToast(new TextBlock() { Text = "Товар добавлен в корзину!", Margin = new Thickness(15, 8) }, 5);
                        }
                        else
                        {
                            // Ошибка при добавлении товара
                            InteractiveContainer.ShowToast(new TextBlock() { Text = "Товар не удалось добавить в корзину!", Margin = new Thickness(15, 8) }, 5);
                        }
                    }
                    break;
                case AccountOutput.ERROR:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "При проверке авторизации произошла ошибка!", Margin = new Thickness(15, 8) }, 5);
                    break;
                case AccountOutput.UNAUTHORIZED:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Пожалуйста, авторизируйтесь!", Margin = new Thickness(15, 8) }, 5);
                    break;
            }

            //HttpClient client = HttpClientInstance.Client;

            //try
            //{
            //    // Проверка авторизации пользователя
            //    var authResponse = await client.GetAsync(URLHelper.APIURL + "/api/account/isauthenticated");
            //    if (authResponse != null && authResponse.ReasonPhrase == "Unauthorized")
            //    {
            //        // Пользователь не авторизован
            //        //MainWindow.getInstance().ShowNotification("Внимание", "Вы не авторизованы");
            //        InteractiveContainer.ShowToast(new TextBlock() { Text = "Пожалуйста, авторизируйтесь!", Margin = new Thickness(15, 8) }, 5);
            //    }
            //    if (authResponse.IsSuccessStatusCode)
            //    {
            //        var authContent = await authResponse.Content.ReadAsStringAsync();
            //        var user = JsonConvert.DeserializeObject<dynamic>(authContent);

            //        // Явное преобразование к типу bool
            //        bool isAuthenticated = user.isAuthenticated.ToObject<bool>();

            //        if (isAuthenticated)
            //        {
            //            // Пользователь авторизован, отправляем товар в корзину
            //            var userID = user.userID.ToObject<int>();
            //            var productCode = SelectedProduct.ProductCode;

            //            var postResponse = await client.PostAsync(
            //                URLHelper.APIURL + $"/api/OrderItem/{userID}.{productCode}",
            //                new StringContent("", Encoding.UTF8, "application/json"));

            //            if (postResponse.IsSuccessStatusCode)
            //            {
            //                // Успешное добавление товара в корзину
            //                InteractiveContainer.ShowToast(new TextBlock() { Text = "Товар добавлен в корзину!", Margin = new Thickness(15, 8) }, 5);
            //            }
            //            else
            //            {
            //                // Ошибка при добавлении товара
            //                InteractiveContainer.ShowToast(new TextBlock() { Text = "Товар не удалось добавить в корзину!", Margin = new Thickness(15, 8) }, 5);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    InteractiveContainer.ShowDialog(new TextBlock() { Text = "Не удалось добавить товар в корзину!", Margin = new Thickness(15, 8) });
            //}

            return null;
        }
    }
}
