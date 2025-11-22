using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnoApp1.Services;
using UnoApp1.Models;
namespace UnoApp1.Presentation;

public partial class CartViewModel : ObservableObject
{
    private readonly ICartService _cartService;
    private readonly INavigator _navigator;
    private readonly IDispatcher _dispatcher; // Để update UI

    [ObservableProperty]
    private ObservableCollection<CartItem> _cartItems;

    [ObservableProperty]
    private decimal _totalPrice; // Mục 3.3: Tổng tiền

    public CartViewModel(ICartService cartService, INavigator navigator, IDispatcher dispatcher)
    {
        _cartService = cartService;
        _navigator = navigator;
        _dispatcher = dispatcher;
        CartItems = new ObservableCollection<CartItem>();

        LoadCartAsync();
    }

    public async void LoadCartAsync()
    {
        var items = await _cartService.GetCartItemsAsync();

        _dispatcher.TryEnqueue(() =>
        {
            CartItems.Clear();
            foreach (var item in items) CartItems.Add(item);
            CalculateTotal(); // Tính tiền ngay khi load
        });
    }

    private void CalculateTotal()
    {
        if (CartItems == null) return;
        // Cộng dồn: Giá * Số lượng của từng món
        TotalPrice = CartItems.Sum(x => x.UnitPrice * x.Quantity);
    }

    [RelayCommand]
    private async Task IncreaseQuantityAsync(CartItem item)
    {
        if (item == null) return;

        item.Quantity++;

        // Lưu xuống DB
        await _cartService.UpdateItemAsync(item);

        // Tính lại tổng tiền
        CalculateTotal();
    }

    [RelayCommand]
    private async Task DecreaseQuantityAsync(CartItem item)
    {
        if (item == null) return;

        if (item.Quantity > 1)
        {
            item.Quantity--; // Giảm
            await _cartService.UpdateItemAsync(item);
        }
        else
        {
            await RemoveItemAsync(item); 
            return; 
        }

        CalculateTotal();
    }

    [RelayCommand]
    private async Task RemoveItemAsync(CartItem item)
    {
        if (item == null) return;

        await _cartService.RemoveItemAsync(item);

        CartItems.Remove(item);

        CalculateTotal();
    }

    // Lệnh chuyển sang trang Thanh toán
    [RelayCommand]
    private async Task GoToCheckoutAsync()
    {
        // Nếu giỏ hàng trống thì không cho đi
        if (CartItems.Count == 0) return;

        await _navigator.NavigateViewModelAsync<CheckoutViewModel>(this);
    }
}
