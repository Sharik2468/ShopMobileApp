using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using InternetShopMobileApp.DTOs;
using InternetShopMobileApp.Services;
using InternetShopMobileApp.Views;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace InternetShopMobileApp.ViewModels
{
    public class AddProductContentViewModel : ReactiveObject, IRoutableViewModel
    {
        private ObservableCollection<CategoryData> _cathegory;
        public ObservableCollection<CategoryData> Cathegory
        {
            get => _cathegory;
            set => this.RaiseAndSetIfChanged(ref _cathegory, value);
        }

        private int _progression = 0;
        public int Progression
        {
            get => _progression;
            set => this.RaiseAndSetIfChanged(ref _progression, value);
        }

        private ProductData _createdProduct;
        public ProductData CreatedProduct
        {
            get => _createdProduct;
            set => this.RaiseAndSetIfChanged(ref _createdProduct, value);
        }

        private int _stepperIndex = 0;
        public int StepperIndex
        {
            get => _stepperIndex;
            set => this.RaiseAndSetIfChanged(ref _stepperIndex, value);
        }

        private string? nameProduct;
        public string? NameProduct
        {
            get => nameProduct;
            set
            {
                CreatedProduct.NameProduct = value;
                this.RaiseAndSetIfChanged(ref nameProduct, value);
                CalculateProgression();
            }
        }
        private int? marketPriceProduct;
        public int? MarketPriceProduct
        {
            get => marketPriceProduct;
            set
            {
                CreatedProduct.MarketPriceProduct = value;
                this.RaiseAndSetIfChanged(ref marketPriceProduct, value);
                CalculateProgression();
            }
        }

        private int? purchasePriceProduct;
        public int? PurchasePriceProduct
        {
            get => purchasePriceProduct;
            set
            {
                CreatedProduct.PurchasePriceProduct = value;
                this.RaiseAndSetIfChanged(ref purchasePriceProduct, value);
                CalculateProgression();
            }
        }

        private DateTimeOffset? dateOfManufactureProduct;
        public DateTimeOffset? DateOfManufactureProduct
        {
            get => dateOfManufactureProduct;
            set
            {
                CreatedProduct.DateOfManufactureProduct = value.Value.DateTime;
                this.RaiseAndSetIfChanged(ref dateOfManufactureProduct, value);
                CalculateProgression();
            }
        }

        private int? bestBeforeDateProduct;
        public int? BestBeforeDateProduct
        {
            get => bestBeforeDateProduct;
            set
            {
                CreatedProduct.BestBeforeDateProduct = value;
                this.RaiseAndSetIfChanged(ref bestBeforeDateProduct, value);
                CalculateProgression();
            }
        }

        private int? categoryId;
        public int? CategoryId
        {
            get => categoryId;
            set
            {
                CreatedProduct.CategoryId = value;
                this.RaiseAndSetIfChanged(ref categoryId, value);
            }
        }

        private string? description;
        public string? Description
        {
            get => description;
            set
            {
                CreatedProduct.Description = value;
                this.RaiseAndSetIfChanged(ref description, value);
                CalculateProgression();
            }
        }

        private byte[]? image;
        public byte[]? Image
        {
            get => image;
            set
            {
                CreatedProduct.Image = value;
                this.RaiseAndSetIfChanged(ref image, value);
                CalculateProgression();
            }
        }

        private int? numberInStock;
        public int? NumberInStock
        {
            get => numberInStock;
            set
            {
                CreatedProduct.NumberInStock = value;
                this.RaiseAndSetIfChanged(ref numberInStock, value);
                CalculateProgression();
            }
        }

        private CategoryData? category;
        public virtual CategoryData? Category
        {
            get => category;
            set
            {
                CreatedProduct.Category = value;
                CreatedProduct.CategoryId = value.CategoryId;
                this.RaiseAndSetIfChanged(ref category, value);
                CalculateProgression();
            }
        }

        public ReactiveCommand<Unit, Unit> AddStepperIndexCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> DecStepperIndexCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> UploadImageCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> AddProductCommand { get; private set; }

        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public AddProductContentViewModel(IScreen screen)
        {
            HostScreen = screen;

            Cathegory = new ObservableCollection<CategoryData>();
            AddStepperIndexCommand = ReactiveCommand.Create(AddIndex);
            DecStepperIndexCommand = ReactiveCommand.Create(DecIndex);
            UploadImageCommand = ReactiveCommand.CreateFromTask(UploadImage);
            AddProductCommand = ReactiveCommand.CreateFromTask(AddProduct);
            CreatedProduct = new ProductData();

            LoadStatuses();
        }

        private void CalculateProgression()
        {
            int filledPropertiesCount = 0;

            if (!string.IsNullOrEmpty(NameProduct)) filledPropertiesCount++;
            if (MarketPriceProduct.HasValue) filledPropertiesCount++;
            if (!string.IsNullOrEmpty(Description)) filledPropertiesCount++;
            if (PurchasePriceProduct.HasValue) filledPropertiesCount++;
            if (DateOfManufactureProduct.HasValue) filledPropertiesCount++;
            if (BestBeforeDateProduct.HasValue) filledPropertiesCount++;
            if (Image != null) filledPropertiesCount++;
            if (NumberInStock.HasValue) filledPropertiesCount++;
            if (Category != null) filledPropertiesCount++;

            Progression = (int)(filledPropertiesCount * 100.0 / 9);
        }


        private async Task UploadImage()
        {
            var dialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Title = "Выберите изображение",
                Filters = new List<FileDialogFilter>
{
    new FileDialogFilter { Name = "Images", Extensions = new List<string> { "jpg", "png", "jpeg" } }
}
            };

            var window = MainWindow.getInstance(); // Примечание: вам нужно получить доступ к текущему окну
            var result = await dialog.ShowAsync(window);
            if (result != null && result.Any())
            {
                var path = result.First();
                Image = await File.ReadAllBytesAsync(path);
            }
        }

        private async Task AddProduct()
        {
            if (Progression < 100)
            {
                InteractiveContainer.ShowToast(new TextBlock() { Text = "Заполните все поля!", Margin = new Thickness(15, 8) }, 5);
                return;
            }

            ProductService service = new ProductService();

            var result = await service.AddProduct(CreatedProduct);

            switch (result.Result)
            {
                case ProductOutput.SUCCESS:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Товар успешно добавлен!", Margin = new Thickness(15, 8) }, 5);
                    HostScreen.Router.Navigate.Execute(new MainContentViewModel(HostScreen));
                    break;
                case ProductOutput.ERROR:
                    InteractiveContainer.ShowToast(new TextBlock() { Text = "Не удалось добавить товар!", Margin = new Thickness(15, 8) }, 5);
                    break;
                case ProductOutput.EXCEPTION: break;
            }
        }

        private async void LoadStatuses()
        {
            ProductService service = new ProductService();

            var result = await service.GetAllCategories();

            switch (result.Result)
            {
                case ProductOutput.SUCCESS:
                    Cathegory.Clear();
                    foreach (var status in result.ProductData)
                        Cathegory.Add(status);
                    break;
                case ProductOutput.ERROR: break;
                case ProductOutput.EXCEPTION: break;
            }
        }

        private void AddIndex()
        {
            if (StepperIndex < 4)
                StepperIndex++;
        }

        private void DecIndex()
        {
            if (StepperIndex > 0)
                StepperIndex--;
        }

    }
}
