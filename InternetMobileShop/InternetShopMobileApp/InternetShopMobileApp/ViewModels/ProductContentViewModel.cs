using Avalonia.Controls;
using Avalonia;
using InternetShopMobileApp.DTOs;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Reactive;
using System.Threading.Tasks;
using InternetShopMobileApp.Services;
using InternetShopMobileApp.DialogueWindows;

namespace InternetShopMobileApp.ViewModels
{
    public class ProductContentViewModel : ReactiveObject, IRoutableViewModel
    {
        public ReactiveCommand<Unit, IRoutableViewModel> AddToBasket { get; }
        public ReactiveCommand<Unit, Unit> DeleteProduct { get; }

        private ProductData _selectedProduct;
        public ProductData SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedProduct, value);
            }
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                this.RaiseAndSetIfChanged(ref _isAdmin, value);
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
            DeleteProduct = ReactiveCommand.Create(DeleteCurrentProduct);
            LoadUser();
        }

        private async void DeleteCurrentProduct()
        {
            ProductService service = new ProductService();

            var result = await service.DeleteProduct(SelectedProduct.ProductCode);

            switch (result.Result)
            {
                case ProductOutput.SUCCESS:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Товар удалён!", Margin = new Thickness(15, 8) }, 5);
                    HostScreen.Router.Navigate.Execute(new MainContentViewModel(HostScreen));
                    break;
                case ProductOutput.ERROR: break;
                case ProductOutput.EXCEPTION: break;
            }
        }

        public ProductContentViewModel(IScreen screen, int productCode)
        {
            HostScreen = screen;
            LoadCurrentProduct(productCode);
            LoadUser();

            // Создайте экземпляр команды и укажите делегат для выполнения
            AddToBasket = ReactiveCommand.CreateFromTask<Unit, IRoutableViewModel>(AddproductToBasket);
            DeleteProduct = ReactiveCommand.Create(DeleteCurrentProduct);
        }
        private async void LoadCurrentProduct(int productCode)
        {
            ProductService service = new ProductService();

            var result = await service.GetProduct(productCode);

            switch (result.Result)
            {
                case ProductOutput.SUCCESS: SelectedProduct = result.ProductData; break;
                case ProductOutput.ERROR: break;
                case ProductOutput.EXCEPTION: break;
            }
        }

        private async void LoadUser()
        {
            AccountService service = new AccountService();

            var result = await service.IsAuthentificated();

            switch (result.Result)
            {
                case AccountOutput.SUCCESS: IsAdmin = result.UserData.userRole.ToObject<string>() == "admin" ? true : false; break;
                case AccountOutput.ERROR: break;
                case AccountOutput.UNAUTHORIZED: IsAdmin = false; break;
            }
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
            return null;
        }
    }
}
