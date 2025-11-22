using System;
using System.Collections.Generic;
using System.Text;
using UnoApp1.Services;
using Refit;
namespace UnoApp1.Presentation;
public partial class CheckoutViewModel : ObservableObject
{
    private readonly ICartService _cartService;
    private readonly INavigator _navigator;

    // Form nhập liệu (Mục 3.4)
    [ObservableProperty] private string _customerName;
    [ObservableProperty] private string _address;
    [ObservableProperty] private string _phoneNumber;

    public CheckoutViewModel(ICartService cartService, INavigator navigator)
    {
        _cartService = cartService;
        _navigator = navigator;
    }

    // Lệnh Đặt hàng (Mục 3.5 & 3.6)
    [RelayCommand]
    private async Task PlaceOrderAsync()
    {
        try
        {
            // 1. Lấy dữ liệu từ SQLite
            var cartItems = await _cartService.GetCartItemsAsync();
            if (cartItems.Count == 0) return;

            decimal total = cartItems.Sum(x => x.UnitPrice * x.Quantity);

            // 2. Đóng gói thành OrderModel (Bước 3.5)
            var order = new OrderModel
            {
                CustomerName = CustomerName,
                Address = Address,
                PhoneNumber = PhoneNumber,
                GrandTotal = total,
                Items = cartItems
            };

            // 3. Gọi API POST (Bước 3.5)
            var apiClient = RestService.For<IApiProduct>("https://69214bcc512fb4140bdfd567.mockapi.io/api/v1");

             await apiClient.CreateOrderAsync(order);

            System.Diagnostics.Debug.WriteLine("Gửi đơn hàng thành công: " + CustomerName);

            await _cartService.ClearCartAsync();

            Console.WriteLine("Đặt hàng thành công!");
            // Điều hướng về trang Main (xóa history để không back lại được)
            await _navigator.NavigateViewModelAsync<MainViewModel>(this, qualifier: Qualifiers.ClearBackStack);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi đặt hàng: " + ex.Message);
        }
    }
}
