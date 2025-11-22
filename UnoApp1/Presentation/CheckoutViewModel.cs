using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Refit;
using UnoApp1.Services;
namespace UnoApp1.Presentation;
public partial class CheckoutViewModel : ObservableValidator
{
    private readonly ICartService _cartService;
    private readonly INavigator _navigator;

    // Form nhập liệu (Mục 3.4)
    [ObservableProperty]
    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    private string _customerName;

    [ObservableProperty]
    private string _errorMessage;


    [ObservableProperty]
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
    private string _address;

    [ObservableProperty]
    [Required(ErrorMessage = "Vui lòng nhập SĐT")]
    [Phone(ErrorMessage = "SĐT không hợp lệ")] // Kiểm tra định dạng số điện thoại
    [RegularExpression(@"^\d{10,}$", ErrorMessage = "SĐT phải là số và đủ 10 số trở lên")]
    private string _phoneNumber;

    [RequiresUnreferencedCode("")]
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
            ValidateAllProperties(); // Ra lệnh kiểm tra toàn bộ các trường

            if (HasErrors) // Nếu có bất kỳ lỗi nào
            {
                ErrorMessage = GetErrors().First().ErrorMessage;
                return;
            }
            ErrorMessage = "";

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
