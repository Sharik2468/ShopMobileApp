using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using InternetShopMobileApp.DialogueWindows;
using InternetShopMobileApp.DTOs;
using InternetShopMobileApp.Services;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace InternetShopMobileApp.ViewModels
{
    public class OrderManagementContentViewModel : ReactiveObject, IRoutableViewModel
    {
        private string _totalOrderSum;
        public string TotalOrderSum
        {
            get => _totalOrderSum;
            set
            {

                this.RaiseAndSetIfChanged(ref _totalOrderSum, value);
            }
        }

        private bool _viewBusy;
        public bool ViewBusy
        {
            get => _viewBusy;
            set
            {
                this.RaiseAndSetIfChanged(ref _viewBusy, value);
            }
        }
        public ReactiveCommand<int, Unit> InfoCommand { get; private set; }
        public ReactiveCommand<int, Unit> DeleteOrderCommand { get; private set; }
        public ReactiveCommand<int, Unit> CompleteOrderCommand { get; private set; }

        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ObservableCollection<OrderData> Orders { get; private set; }

        public OrderManagementContentViewModel(IScreen screen)
        {
            HostScreen = screen;
            Orders = new ObservableCollection<OrderData>();
            InfoCommand = ReactiveCommand.CreateFromTask<int>(AdditionalInfo);
            DeleteOrderCommand = ReactiveCommand.CreateFromTask<int>(DeleteOrder);
            CompleteOrderCommand = ReactiveCommand.CreateFromTask<int>(CompleteOrder);

            LoadOrderForManagement();
            ViewBusy = true;
        }

        private async Task AdditionalInfo(int productCode)
        {
            HostScreen.Router.Navigate.Execute(new ProductContentViewModel(HostScreen, productCode));
        }

        private async Task DeleteOrder(int orderCode)
        {
            OrderService service = new OrderService();

            var result = await service.DeleteOrder(orderCode);

            switch (result.Result)
            {
                case OrderOutput.SUCCESS:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Заказ успешно удалён!", Margin = new Thickness(15, 8) }, 5);
                    Orders.Clear();
                    ViewBusy = true;
                    LoadOrderForManagement();
                    break;
                case OrderOutput.ERROR: break;
                case OrderOutput.EXCEPTION: break;
            }
        }

        private async Task CompleteOrder(int orderCode)
        {
            AccountService serviceAccount = new AccountService();
            OrderService serviceOrders = new OrderService();

            var resultAccount = await serviceAccount.IsAuthentificated();

            switch (resultAccount.Result)
            {
                case AccountOutput.SUCCESS:

                    bool isAuthenticated = resultAccount.UserData.isAuthenticated.ToObject<bool>();
                    string userRole = resultAccount.UserData.userRole.ToObject<string>();
                    // Предположим, у вас есть способ получения ID пользователя
                    var userID = resultAccount.UserData.userID.ToObject<int>();
                    if (userID != null && isAuthenticated && userRole == "user")
                    {
                        InteractiveContainer.ShowDialog(new RolesErrorDialogContent());
                        HostScreen.Router.Navigate.Execute(new MainContentViewModel(HostScreen));
                        break;
                    }

                    if (userID != null && isAuthenticated)
                    {
                        var resultOrderItems = await serviceOrders.PayOrder(orderCode, userID);
                        if (resultOrderItems != null && resultOrderItems.Result == OrderOutput.SUCCESS)
                        {
                            InteractiveContainer.ShowToast(new TextBlock() { Text = "Товар успешно оплачен!", Margin = new Thickness(15, 8) }, 5);
                            ViewBusy = true;
                            Orders.Clear();
                            LoadOrderForManagement();
                        }
                        else if (resultOrderItems.Result == BasketOutput.ERROR)
                        {
                            InteractiveContainer.ShowToast(new TextBlock() { Text = "Не получилось оплатить товар!", Margin = new Thickness(15, 8) }, 5);
                        }
                    }
                    break;

                case AccountOutput.UNAUTHORIZED:
                    Dispatcher.UIThread.Post(() =>
                    {
                        InteractiveContainer.ShowDialog(new ErrorDialogContent());
                        HostScreen.Router.Navigate.Execute(new MainContentViewModel(HostScreen));
                    });
                    break;
            }
        }

        private async void LoadOrderForManagement()
        {
            AccountService serviceAccount = new AccountService();
            OrderService serviceOrder = new OrderService();

            var resultAccount = await serviceAccount.IsAuthentificated();

            switch (resultAccount.Result)
            {
                case AccountOutput.SUCCESS:

                    bool isAuthenticated = resultAccount.UserData.isAuthenticated.ToObject<bool>();
                    string userRole = resultAccount.UserData.userRole.ToObject<string>();
                    // Предположим, у вас есть способ получения ID пользователя
                    var userID = resultAccount.UserData.userID.ToObject<int>();
                    if (userID != null && isAuthenticated && userRole == "user")
                    {
                        InteractiveContainer.ShowDialog(new RolesErrorDialogContent());
                        HostScreen.Router.Navigate.Execute(new MainContentViewModel(HostScreen));
                        break;
                    }

                    if (userID != null && isAuthenticated)
                    {
                        var resultOrders = await serviceOrder.GetAllPreparedOrders();
                        if (resultOrders != null && resultOrders.Result == OrderOutput.SUCCESS)
                        {
                            foreach (var item in resultOrders.OrderData)
                            {
                                Orders.Add(item);
                            }
                            ViewBusy = false;
                        }
                        else if (resultOrders.Result == OrderOutput.ERROR)
                        {
                            InteractiveContainer.ShowToast(new TextBlock() { Text = "Нет заказов!", Margin = new Thickness(15, 8) }, 5);
                        }
                    }
                    break;

                case AccountOutput.UNAUTHORIZED:
                    Dispatcher.UIThread.Post(() =>
                    {
                        InteractiveContainer.ShowDialog(new ErrorDialogContent());
                        HostScreen.Router.Navigate.Execute(new MainContentViewModel(HostScreen));
                    });
                    break;
            }
        }
    }
}
