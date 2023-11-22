using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using InternetShopMobileApp.DTOs;
using Newtonsoft.Json;
using ReactiveUI;
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

namespace InternetShopMobileApp.ViewModels
{
    public class ProductContentViewModel : ReactiveObject, IRoutableViewModel
    {
        WindowNotificationManager notificationManager;
        public ReactiveCommand<Unit, IRoutableViewModel> ShowNotify { get; }

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

        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ProductContentViewModel(IScreen screen, ProductData productData = null)
        {
            HostScreen = screen;
            SelectedProduct = productData;
        }

        private void ShowNotification()
        {
            var notif = new Avalonia.Controls.Notifications.Notification("title", "message");
            notificationManager.Show(notif);
        }
    }
}
