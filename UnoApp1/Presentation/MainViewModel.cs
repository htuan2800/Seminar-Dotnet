using System.Collections.ObjectModel;
using Refit;
using UnoApp1.Services;
namespace UnoApp1.Presentation;
using CommunityToolkit.Mvvm.Input;
using Uno.Extensions;

public partial class MainViewModel : ObservableObject
{
    private INavigator _navigator;

    private IDispatcher _dispatcher;

    private ICartService _cartService;

    //Khai báo biến IsLoading
    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private int _cartCount;

    // Danh sách sản phẩm
    [ObservableProperty]
    private ObservableCollection<Product> _products;

    public MainViewModel(INavigator navigator, IDispatcher dispatcher, ICartService cartService)
    {
        _navigator = navigator;
        _dispatcher = dispatcher;
        Title = "Danh sách sản phẩm";

        Products = new ObservableCollection<Product>();

        LoadDataAsync();
        _cartService = cartService;

        UpdateBadge();
    }


    public async void UpdateBadge()
    {
        CartCount = await _cartService.GetCartCountAsync();
    }

    public string Title { get; }

    // 4. Tách logic gọi API ra hàm riêng
    private async void LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;

            // Link API Mock
            string baseUrl = "https://69214bcc512fb4140bdfd567.mockapi.io/api/v1";

            var apiClient = RestService.For<IApiProduct>(baseUrl);

            // Gọi API
            var listFromApi = await apiClient.GetProductsAsync();

            Console.WriteLine($"KẾT QUẢ API: Tìm thấy {listFromApi.Count} sản phẩm");

            //Chuyển về luồng UI để vẽ lên màn hình
            _dispatcher.TryEnqueue(() =>
            {
                Products.Clear();
                foreach (var item in listFromApi)
                {
                    Products.Add(item);
                }

                IsLoading = false;
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi rồi: {ex.Message}");

            // Nếu lỗi cũng phải chuyển về UI thread mới tắt Loading được
            _dispatcher.TryEnqueue(() => IsLoading = false);
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Lệnh mở trang Giỏ hàng
    [RelayCommand]
    private async Task GoToCartAsync()
    {
         await _navigator.NavigateViewModelAsync<CartViewModel>(this);
    }

    // Lệnh mở trang Chi tiết
    [RelayCommand]
    private async Task GoToDetailAsync(Product selectedProduct)
    {
        if (selectedProduct == null)
        {
            Console.WriteLine("selectedProduct is NULL!");
            return;
        }

        Console.WriteLine($"Đang chuyển: {selectedProduct.product_name}");

        ProductDetailViewModel.TempProductPayload = selectedProduct;

        // Navigate KHÔNG truyền data parameter
        await _navigator.NavigateViewModelAsync<ProductDetailViewModel>(this);
    }
}
