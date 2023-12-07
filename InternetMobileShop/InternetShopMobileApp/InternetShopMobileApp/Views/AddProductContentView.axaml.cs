using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using InternetShopMobileApp.DTOs;
using InternetShopMobileApp.ViewModels;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InternetShopMobileApp.Views
{
    public partial class AddProductContentView : ReactiveUserControl<AddProductContentViewModel>
    {
        public AddProductContentView()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);

            this.FindControl<Stepper>("myStep").Steps = new ObservableCollection<string>() { "Описательной информации", "Даты", "Цены", "Доп. Информация", "Подтверждение" };
            this.FindControl<Stepper>("myStep").Index = 0;
        }
    }
}
