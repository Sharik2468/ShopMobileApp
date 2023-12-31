﻿// <auto-generated />
using System;
using InternetShopWebApp.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InternetShopWebApp.Migrations
{
    [DbContext(typeof(InternetShopContext))]
    partial class InternetShopContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("InternetShopWebApp.Models.CategoryTable", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("Category_ID");

                    b.Property<string>("CategoryName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Category_Name");

                    b.Property<int>("ParentId")
                        .HasColumnType("int")
                        .HasColumnName("Parent_ID");

                    b.HasKey("CategoryId");

                    b.HasIndex("ParentId");

                    b.ToTable("Category_Table", (string)null);
                });

            modelBuilder.Entity("InternetShopWebApp.Models.ClientTable", b =>
                {
                    b.Property<int>("ClientCode")
                        .HasColumnType("int")
                        .HasColumnName("Client_Code");

                    b.Property<int?>("LocationCode")
                        .HasColumnType("int")
                        .HasColumnName("Location_Code");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("TelephoneNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("Telephone_Number");

                    b.HasKey("ClientCode");

                    b.HasIndex("LocationCode");

                    b.ToTable("Client_Table", (string)null);
                });

            modelBuilder.Entity("InternetShopWebApp.Models.LocationTable", b =>
                {
                    b.Property<int>("LocationCode")
                        .HasColumnType("int")
                        .HasColumnName("Location_Code");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocationCode");

                    b.ToTable("Location_Table", (string)null);
                });

            modelBuilder.Entity("InternetShopWebApp.Models.OrderItemTable", b =>
                {
                    b.Property<int>("OrderItemCode")
                        .HasColumnType("int")
                        .HasColumnName("Order_Item_Code");

                    b.Property<int?>("AmountOrderItem")
                        .HasColumnType("int")
                        .HasColumnName("Amount_Order_Item");

                    b.Property<int?>("OrderCode")
                        .HasColumnType("int")
                        .HasColumnName("Order_Code");

                    b.Property<int?>("OrderSum")
                        .HasColumnType("int")
                        .HasColumnName("Order_Sum");

                    b.Property<int?>("ProductCode")
                        .HasColumnType("int")
                        .HasColumnName("Product_Code");

                    b.Property<int>("StatusOrderItemTableId")
                        .HasColumnType("int")
                        .HasColumnName("Status_Order_Item_Table_ID");

                    b.HasKey("OrderItemCode");

                    b.HasIndex("OrderCode");

                    b.HasIndex("ProductCode");

                    b.HasIndex("StatusOrderItemTableId");

                    b.ToTable("Order_Item_Table", (string)null);
                });

            modelBuilder.Entity("InternetShopWebApp.Models.OrderTable", b =>
                {
                    b.Property<int>("OrderCode")
                        .HasColumnType("int")
                        .HasColumnName("Order_Code");

                    b.Property<int?>("ClientCode")
                        .HasColumnType("int")
                        .HasColumnName("Client_Code");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("date")
                        .HasColumnName("Order_Date");

                    b.Property<DateTime?>("OrderFullfillment")
                        .HasColumnType("date")
                        .HasColumnName("Order_Fullfillment");

                    b.Property<int?>("SalesmanCode")
                        .HasColumnType("int")
                        .HasColumnName("Salesman_Code");

                    b.Property<int?>("StatusCode")
                        .HasColumnType("int")
                        .HasColumnName("Status_Code");

                    b.HasKey("OrderCode");

                    b.HasIndex("ClientCode");

                    b.HasIndex("SalesmanCode");

                    b.HasIndex("StatusCode");

                    b.ToTable("Order_Table", (string)null);
                });

            modelBuilder.Entity("InternetShopWebApp.Models.ProductTable", b =>
                {
                    b.Property<int>("ProductCode")
                        .HasColumnType("int")
                        .HasColumnName("Product_Code");

                    b.Property<int?>("BestBeforeDateProduct")
                        .HasColumnType("int")
                        .HasColumnName("Best_Before_Date_Product");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("Category_ID");

                    b.Property<DateTime?>("DateOfManufactureProduct")
                        .HasColumnType("date")
                        .HasColumnName("Date_of_Manufacture_Product");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("MarketPriceProduct")
                        .HasColumnType("int")
                        .HasColumnName("Market_Price_Product");

                    b.Property<string>("NameProduct")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name_Product");

                    b.Property<int?>("NumberInStock")
                        .HasColumnType("int")
                        .HasColumnName("Number_in_Stock");

                    b.Property<int?>("PurchasePriceProduct")
                        .HasColumnType("int")
                        .HasColumnName("Purchase_Price_Product");

                    b.HasKey("ProductCode");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product_Table", (string)null);
                });

            modelBuilder.Entity("InternetShopWebApp.Models.SalesmanTable", b =>
                {
                    b.Property<int>("SalesmanCode")
                        .HasColumnType("int")
                        .HasColumnName("Salesman_Code");

                    b.Property<int?>("LocationCode")
                        .HasColumnType("int")
                        .HasColumnName("Location_Code");

                    b.Property<string>("Password")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SalemanName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Saleman_Name");

                    b.Property<string>("SalesmanSurname")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Salesman_Surname");

                    b.Property<long?>("TelephoneNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("Telephone_Number");

                    b.HasKey("SalesmanCode");

                    b.HasIndex("LocationCode");

                    b.ToTable("Salesman_Table", (string)null);
                });

            modelBuilder.Entity("InternetShopWebApp.Models.StatusOrderItemTable", b =>
                {
                    b.Property<int>("StatusOrderItemId")
                        .HasColumnType("int")
                        .HasColumnName("Status_Order_Item_ID");

                    b.Property<string>("StatusOrderItemTable1")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Status_Order_Item_Table");

                    b.HasKey("StatusOrderItemId");

                    b.ToTable("Status_Order_Item_Table", (string)null);
                });

            modelBuilder.Entity("InternetShopWebApp.Models.StatusTable", b =>
                {
                    b.Property<int>("StatusCode")
                        .HasColumnType("int")
                        .HasColumnName("Status_Code");

                    b.Property<string>("OrderStatus")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .HasColumnName("Order_Status")
                        .IsFixedLength();

                    b.HasKey("StatusCode")
                        .HasName("PK_Delivery_Table");

                    b.ToTable("Status_Table", (string)null);
                });

            modelBuilder.Entity("InternetShopWebApp.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("NormalCode")
                        .HasColumnType("int");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("InternetShopWebApp.Models.CategoryTable", b =>
                {
                    b.HasOne("InternetShopWebApp.Models.CategoryTable", "Parent")
                        .WithMany("InverseParent")
                        .HasForeignKey("ParentId")
                        .IsRequired()
                        .HasConstraintName("FK_Category_Table_Category_Table");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.ClientTable", b =>
                {
                    b.HasOne("InternetShopWebApp.Models.LocationTable", "LocationCodeNavigation")
                        .WithMany("ClientTables")
                        .HasForeignKey("LocationCode")
                        .HasConstraintName("FK_Client_Table_Location_Table");

                    b.Navigation("LocationCodeNavigation");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.OrderItemTable", b =>
                {
                    b.HasOne("InternetShopWebApp.Models.OrderTable", "OrderCodeNavigation")
                        .WithMany("OrderItemTables")
                        .HasForeignKey("OrderCode")
                        .HasConstraintName("FK_Order_Item_Table_Order_Table");

                    b.HasOne("InternetShopWebApp.Models.ProductTable", "ProductCodeNavigation")
                        .WithMany("OrderItemTables")
                        .HasForeignKey("ProductCode")
                        .HasConstraintName("FK_Order_Item_Table_Product_Table");

                    b.HasOne("InternetShopWebApp.Models.StatusOrderItemTable", "StatusOrderItemTable")
                        .WithMany("OrderItemTables")
                        .HasForeignKey("StatusOrderItemTableId")
                        .IsRequired()
                        .HasConstraintName("FK_Order_Item_Table_Status_Order_Item_Table");

                    b.Navigation("OrderCodeNavigation");

                    b.Navigation("ProductCodeNavigation");

                    b.Navigation("StatusOrderItemTable");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.OrderTable", b =>
                {
                    b.HasOne("InternetShopWebApp.Models.ClientTable", "ClientCodeNavigation")
                        .WithMany("OrderTables")
                        .HasForeignKey("ClientCode")
                        .HasConstraintName("FK_Order_Table_Client_Table");

                    b.HasOne("InternetShopWebApp.Models.SalesmanTable", "SalesmanCodeNavigation")
                        .WithMany("OrderTables")
                        .HasForeignKey("SalesmanCode")
                        .HasConstraintName("FK_Order_Table_Salesman_Table");

                    b.HasOne("InternetShopWebApp.Models.StatusTable", "StatusCodeNavigation")
                        .WithMany("OrderTables")
                        .HasForeignKey("StatusCode")
                        .HasConstraintName("FK_Order_Table_Delivery_Table");

                    b.Navigation("ClientCodeNavigation");

                    b.Navigation("SalesmanCodeNavigation");

                    b.Navigation("StatusCodeNavigation");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.ProductTable", b =>
                {
                    b.HasOne("InternetShopWebApp.Models.CategoryTable", "Category")
                        .WithMany("ProductTables")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK_Product_Table_Category_Table");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.SalesmanTable", b =>
                {
                    b.HasOne("InternetShopWebApp.Models.LocationTable", "LocationCodeNavigation")
                        .WithMany("SalesmanTables")
                        .HasForeignKey("LocationCode")
                        .HasConstraintName("FK_Salesman_Table_Location_Table");

                    b.Navigation("LocationCodeNavigation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("InternetShopWebApp.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("InternetShopWebApp.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InternetShopWebApp.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("InternetShopWebApp.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InternetShopWebApp.Models.CategoryTable", b =>
                {
                    b.Navigation("InverseParent");

                    b.Navigation("ProductTables");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.ClientTable", b =>
                {
                    b.Navigation("OrderTables");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.LocationTable", b =>
                {
                    b.Navigation("ClientTables");

                    b.Navigation("SalesmanTables");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.OrderTable", b =>
                {
                    b.Navigation("OrderItemTables");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.ProductTable", b =>
                {
                    b.Navigation("OrderItemTables");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.SalesmanTable", b =>
                {
                    b.Navigation("OrderTables");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.StatusOrderItemTable", b =>
                {
                    b.Navigation("OrderItemTables");
                });

            modelBuilder.Entity("InternetShopWebApp.Models.StatusTable", b =>
                {
                    b.Navigation("OrderTables");
                });
#pragma warning restore 612, 618
        }
    }
}
