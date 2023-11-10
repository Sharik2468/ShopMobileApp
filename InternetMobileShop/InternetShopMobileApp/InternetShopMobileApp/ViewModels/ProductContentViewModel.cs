﻿using InternetShopMobileApp.DTOs;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShopMobileApp.ViewModels
{
    public class ProductContentViewModel : ReactiveObject, IRoutableViewModel
    {
        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ProductContentViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
