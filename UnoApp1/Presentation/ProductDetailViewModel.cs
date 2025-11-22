using System;
using System.Collections.Generic;
using System.Text;
using UnoApp1.Services;
using UnoApp1.Models;
using Uno.Extensions.Navigation;
namespace UnoApp1.Presentation;
using Microsoft.UI.Xaml.Data;
[Bindable]
public partial class ProductDetailViewModel : ObservableObject
{
    private ICartService _cartService; // Khai báo Service

    public static Product? TempProductPayload;

    [ObservableProperty]
    private Product _product;

    [ObservableProperty]
    private int _quantity = 1;

    public decimal TotalPrice => Product != null ? Product.price * Quantity : 0;

    // Thêm constructor nhận Product từ navigation
    public ProductDetailViewModel(ICartService cartService, Product? product = null)
    {
        _cartService = cartService;

        Console.WriteLine("Constructor được gọi");

        if (TempProductPayload != null)
        {
            Console.WriteLine($"✅ Dữ liệu từ static: {TempProductPayload.product_name}");
            Product = TempProductPayload;
            TempProductPayload = null; // Xóa sau khi dùng
        }
        else
        {
            Console.WriteLine("❌ TempProductPayload là null");
        }
    }

    // TĂNG SỐ LƯỢNG
    [RelayCommand]
    private void IncreaseQuantity()
    {
        Quantity++;
    }

    // GIẢM SỐ LƯỢNG
    [RelayCommand]
    private void DecreaseQuantity()
    {
        if (Quantity > 1)
            Quantity--;
    }

    partial void OnQuantityChanged(int value)
    {
        OnPropertyChanged(nameof(TotalPrice));
    }

    // ⭐ SỬA LẠI AddToCart - THÊM NHIỀU LẦN THEO QUANTITY
    [RelayCommand]
    private async Task AddToCartAsync()
    {
        if (Product == null) return;

        // Thêm với số lượng đã chọn
        await _cartService.AddItemAsync(Product, Quantity);

        Console.WriteLine($"Đã thêm {Quantity} {Product.product_name} vào giỏ!");

        // Reset về 1 sau khi thêm
        Quantity = 1;
    }
}
