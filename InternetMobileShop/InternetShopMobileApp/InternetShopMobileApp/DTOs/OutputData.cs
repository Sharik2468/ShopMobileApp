using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace InternetShopMobileApp.DTOs
{
    public class ProductData
    {
        public int ProductCode { get; set; }

        public string? NameProduct { get; set; }

        public int? MarketPriceProduct { get; set; }

        public int? PurchasePriceProduct { get; set; }

        public DateTime? DateOfManufactureProduct { get; set; }

        public int? BestBeforeDateProduct { get; set; }

        public int? CategoryId { get; set; }

        public string? Description { get; set; }

        public byte[]? Image { get; set; }

        public int? NumberInStock { get; set; }

        public virtual CategoryData? Category { get; set; }

        public virtual ICollection<OrderItemData> OrderItemDatas { get; } = new List<OrderItemData>();
    }

    public partial class ClientData
    {
        public int ClientCode { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string Roles { get; set; }

        public long? TelephoneNumber { get; set; }

        public string? Password { get; set; }

        public int? LocationCode { get; set; }

        public virtual LocationData? LocationCodeNavigation { get; set; }

        public virtual ICollection<OrderData> OrderDatas { get; } = new List<OrderData>();
    }

    public partial class CategoryData
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public int ParentId { get; set; }

        public virtual ICollection<CategoryData> InverseParent { get; } = new List<CategoryData>();

        public virtual CategoryData Parent { get; set; } = null!;

        public virtual ICollection<ProductData> ProductDatas { get; } = new List<ProductData>();
    }

    public partial class LocationData
    {
        public int LocationCode { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<ClientData> ClientDatas { get; } = new List<ClientData>();

        public virtual ICollection<SalesmanData> SalesmanDatas { get; } = new List<SalesmanData>();
    }

    public partial class OrderItemData : INotifyPropertyChanged
    {
        public int OrderItemCode { get; set; }
        public int? OrderSum { get; set; }
        public string? OrderSumString { get; set; }
        public int? AmountOrderItem { get; set; }
        public string? AmountString { get; set; }
        public int? ProductCode { get; set; }
        public int? OrderCode { get; set; }
        public int? StatusOrderItemTableId { get; set; }
        public string? StatusOrderItemDataName { get; set; }
        public virtual OrderData? OrderCodeNavigation { get; set; }
        public virtual ProductData? ProductCodeNavigation { get; set; }
        public virtual StatusOrderItemData StatusOrderItemData { get; set; } = null!;

        public event PropertyChangedEventHandler PropertyChanged;

        private double _visibleAmount = 1;
        public double VisibleAmount
        {
            get => _visibleAmount;
            set
            {
                _visibleAmount = value;
                OnPropertyChanged("VisibleAmount");
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public partial class OrderData
    {
        public int OrderCode { get; set; }

        public DateTime? OrderFullfillment { get; set; }

        public DateTime? OrderDate { get; set; }

        public int? ClientCode { get; set; }

        public int? SalesmanCode { get; set; }

        public int? StatusCode { get; set; }

        public virtual ClientData? ClientCodeNavigation { get; set; }

        public virtual ICollection<OrderItemData> OrderItemTables { get; } = new List<OrderItemData>();

        public virtual SalesmanData? SalesmanCodeNavigation { get; set; }

        public virtual StatusData? StatusCodeNavigation { get; set; }
    }

    public partial class SalesmanData
    {
        public int SalesmanCode { get; set; }

        public string? SalemanName { get; set; }

        public string? SalesmanSurname { get; set; }

        public long? TelephoneNumber { get; set; }

        public string? Password { get; set; }

        public int? LocationCode { get; set; }

        public virtual LocationData? LocationCodeNavigation { get; set; }

        public virtual ICollection<OrderData> OrderDatas { get; } = new List<OrderData>();
    }

    public partial class StatusOrderItemData
    {
        [Key]
        public int StatusOrderItemId { get; set; }

        public string? StatusOrderItemData1 { get; set; }

        public virtual ICollection<OrderItemData> OrderItemDatas { get; } = new List<OrderItemData>();
    }

    public partial class StatusData
    {
        [Key]
        public int StatusCode { get; set; }

        public string? OrderStatus { get; set; }

        public virtual ICollection<OrderData> OrderDatas { get; } = new List<OrderData>();
    }
}
