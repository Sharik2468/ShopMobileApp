﻿using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShopMobileApp.DTOs
{
    public class ProductDTO
    {
        public Bitmap? ImageSource { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string Availability { get; set; }
    }
}
