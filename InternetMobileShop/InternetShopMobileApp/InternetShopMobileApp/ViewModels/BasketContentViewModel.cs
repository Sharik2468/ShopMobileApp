using Avalonia.Controls;
using Avalonia;
using InternetShopMobileApp.DTOs;
using InternetShopMobileApp.Views;
using Newtonsoft.Json;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using InternetShopMobileApp.Services;
using InternetShopMobileApp.DialogueWindows;

namespace InternetShopMobileApp.ViewModels
{
    public class BasketContentViewModel : ReactiveObject, IRoutableViewModel
    {
        private OrderItemData _selectedOrder;
        public OrderItemData SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedOrder, value);
                if (SelectedOrder != null)
                    DeleteOrderItem(SelectedOrder.OrderItemCode);
            }
        }

        private double _orderItemAmount = 1;
        public double OrderItemAmount
        {
            get => _orderItemAmount;
            set
            {
                this.RaiseAndSetIfChanged(ref _orderItemAmount, value);
            }
        }

        private string _orderItemCount;
        public string OrderItemCount
        {
            get => _orderItemCount;
            set
            {
                int? sum = 0;
                foreach (var item in OrderItems)
                    sum += item.AmountOrderItem;
                this.RaiseAndSetIfChanged(ref _orderItemCount, $"{sum} товаров");
            }
        }

        private string _orderItemSum;
        public string OrderItemSum
        {
            get => _orderItemSum;
            set
            {
                int? sum = 0;
                foreach (var item in OrderItems)
                    if (item.StatusOrderItemTableId == 1)
                        sum += item.OrderSum;
                this.RaiseAndSetIfChanged(ref _orderItemSum, $"{sum} р.");
            }
        }

        private bool _viewBusy;
        public bool ViewBusy
        {
            get => _viewBusy;
            set
            {
                this.RaiseAndSetIfChanged(ref _viewBusy, OrderItems.Count() == 0);
            }
        }

        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ObservableCollection<OrderItemData> OrderItems { get; private set; }
        public ObservableCollection<StatusData> Statuses { get; private set; }
        public ReactiveCommand<int, Unit> DeleteOrderItemCommand { get; private set; }
        public ReactiveCommand<int, Unit> ChangeAmountOrderItemCommand { get; private set; }
        public ReactiveCommand<int, Unit> ChangeStatusOrderItemCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> PrepareOrderCommand { get; private set; }

        public BasketContentViewModel(IScreen screen)
        {
            HostScreen = screen;
            OrderItems = new ObservableCollection<OrderItemData>();
            Statuses = new ObservableCollection<StatusData>();
            LoadOrderItems();

            DeleteOrderItemCommand = ReactiveCommand.CreateFromTask<int>(DeleteOrderItem);
            ChangeAmountOrderItemCommand = ReactiveCommand.CreateFromTask<int>(ChangeAmountOrderItem);
            ChangeStatusOrderItemCommand = ReactiveCommand.CreateFromTask<int>(ChangeStatusOrderItem);
            PrepareOrderCommand = ReactiveCommand.CreateFromTask(PrepareOrder);

            ViewBusy = false;
        }

        private async void LoadOrderItems()
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
                        HostScreen.Router.Navigate.Execute(new MainContentViewModel(HostScreen));
                        break;
                    }

                    if (userID != null && isAuthenticated)
                    {
                        var resultOrderItems = await serviceBasket.LoadItems(userID);
                        var resultStatuses = await serviceBasket.GetAllOrderItemStatuses();
                        if (resultOrderItems != null && resultOrderItems.Result == BasketOutput.SUCCESS)
                        {
                            foreach (var item in resultStatuses.BasketData)
                            {
                                StatusData newStatus = new StatusData
                                {
                                    StatusCode = item.statusOrderItemId.ToObject<int>(),
                                    OrderStatus = item.statusOrderItemTable1.ToObject<string>(),
                                };
                                Statuses.Add(newStatus);
                            }

                            foreach (var item in resultOrderItems.BasketData)
                            {
                                item.OrderSumString = $"{item.OrderSum} р.";
                                item.AmountString = $"Количество: {item.AmountOrderItem} шт.";
                                item.StatusOrderItemDataName = Statuses[item.StatusOrderItemTableId - 1].OrderStatus;
                                OrderItems.Add(item);
                            }
                            OrderItemCount = "";
                            OrderItemSum = "";
                            ViewBusy = false;
                        }
                        else if (resultOrderItems.Result == BasketOutput.ERROR)
                        {
                            InteractiveContainer.ShowToast(new TextBlock() { Text = "Добавьте товары в корзину!", Margin = new Thickness(15, 8) }, 5);
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

        // Метод для удаления элемента заказа
        private async Task DeleteOrderItem(int orderItemCode)
        {
            BasketService serviceBasket = new BasketService();

            var responese = await serviceBasket.DeleteOrderItem(orderItemCode);

            switch (responese.Result)
            {
                case BasketOutput.SUCCESS:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Элемент заказа удалён!", Margin = new Thickness(15, 8) }, 5);
                    //OrderItems.Clear();
                    //LoadOrderItems();

                    OrderItems.Remove(OrderItems.FirstOrDefault(a => a.OrderItemCode == orderItemCode));
                    OrderItemCount = "";
                    OrderItemSum = "";
                    ViewBusy = false;
                    break;

                case BasketOutput.ERROR:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Не удалось удалить элемент заказа!", Margin = new Thickness(15, 8) }, 5);
                    break;

                case BasketOutput.EXCEPTION:
                    InteractiveContainer.ShowDialog(new TextBlock() { Text = "Не удалось удалить элемент заказа!", Margin = new Thickness(15, 8) });
                    break;
            }

        }
        private async Task ChangeStatusOrderItem(int orderItemID)
        {
            BasketService serviceBasket = new BasketService();

            OrderItemData orderItem = OrderItems.Where(a => a.OrderItemCode == orderItemID).FirstOrDefault();
            var responese = await serviceBasket.UpdateOrderItemStatus(orderItemID, orderItem.StatusOrderItemTableId == 1 ? 2 : 1);

            switch (responese.Result)
            {
                case BasketOutput.SUCCESS:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Состояние товара изменено!", Margin = new Thickness(15, 8) }, 5);
                    OrderItems.Clear();
                    ViewBusy = true;
                    LoadOrderItems();
                    break;

                case BasketOutput.ERROR:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Не удалось изменить состояние заказа!", Margin = new Thickness(15, 8) }, 5);
                    break;

                case BasketOutput.EXCEPTION:
                    InteractiveContainer.ShowDialog(new TextBlock() { Text = "Не удалось изменить состояние заказа!", Margin = new Thickness(15, 8) });
                    break;
            }
        }

        private async Task ChangeAmountOrderItem(int orderItemID)
        {
            BasketService serviceBasket = new BasketService();

            var responese = await serviceBasket.UpdateOrderItemQuantity(orderItemID, ((int)OrderItemAmount));

            switch (responese.Result)
            {
                case BasketOutput.SUCCESS:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Количество товара изменено!", Margin = new Thickness(15, 8) }, 5);
                    OrderItems.Clear();
                    ViewBusy = true;
                    LoadOrderItems();
                    break;

                case BasketOutput.ERROR:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Не удалось изменить элемент заказа!", Margin = new Thickness(15, 8) }, 5);
                    break;

                case BasketOutput.EXCEPTION:
                    InteractiveContainer.ShowDialog(new TextBlock() { Text = "Не удалось изменить элемент заказа!", Margin = new Thickness(15, 8) });
                    break;
            }
        }

        private async Task PrepareOrder()
        {
            BasketService serviceBasket = new BasketService();

            var responese = await serviceBasket.PrepareOrder(OrderItems[0].OrderCode);

            switch (responese.Result)
            {
                case BasketOutput.SUCCESS:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Заказ оформлен!", Margin = new Thickness(15, 8) }, 5);
                    OrderItems.Clear();
                    ViewBusy = true;
                    LoadOrderItems();
                    break;

                case BasketOutput.ERROR:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Не удалось оформить заказ!", Margin = new Thickness(15, 8) }, 5);
                    break;

                case BasketOutput.EXCEPTION:
                    InteractiveContainer.ShowDialog(new TextBlock() { Text = "Не удалось оформить заказ!", Margin = new Thickness(15, 8) });
                    break;
            }
        }
    }
}
