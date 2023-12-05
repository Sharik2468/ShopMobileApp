using InternetShopMobileApp.ViewModels;
using InternetShopMobileApp.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShopMobileApp
{
    public class AppViewLocator : ReactiveUI.IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
        {
            MainContentViewModel mainPageViewModel => new MainContentView { DataContext = mainPageViewModel },
            ProductContentViewModel productPageViewModel => new ProductContentView { DataContext = productPageViewModel },
            ProfileContentViewModel profilePageViewModel => new ProfileContentView { DataContext = profilePageViewModel },
            CatalogContentViewModel catalogPageViewModel => new CatalogContentView { DataContext = catalogPageViewModel },
            BasketContentViewModel basketPageViewModel => new BasketContentView { DataContext = basketPageViewModel },
            ProfileLoginContentViewModel profileLoginViewModel => new ProfileLoginContentView { DataContext = profileLoginViewModel },
            PreparedContentViewModel preparedContentViewModel => new PreparedContentView { DataContext = preparedContentViewModel },
            OrderManagementContentViewModel orderManagementContentViewModel => new OrderManagementContentView { DataContext = orderManagementContentViewModel },
            AddProductContentViewModel addProductContentViewModel => new AddProductContentView { DataContext = addProductContentViewModel },
            // ... добавьте другие модели представлений и представления здесь ...
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel), $"Could not find view for view model type {viewModel.GetType()}."),
        };

    }
}
