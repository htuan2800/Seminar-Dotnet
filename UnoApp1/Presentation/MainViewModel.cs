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

    // 1. Khai báo biến IsLoading (Bạn bị thiếu cái này)
    [ObservableProperty]
    private bool _isLoading;

    // Danh sách sản phẩm
    [ObservableProperty]
    private ObservableCollection<Product> _products;

    public MainViewModel(INavigator navigator, IDispatcher dispatcher)
    {
        _navigator = navigator;
        _dispatcher = dispatcher;
        Title = "Danh sách sản phẩm";

        Products = new ObservableCollection<Product>();

        // 3. Gọi hàm tải dữ liệu (Fire-and-forget)
        // Vì Constructor không thể await, ta gọi hàm async mà không cần await ở đây
        LoadDataAsync();
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

            Console.WriteLine($"👉 KẾT QUẢ API: Tìm thấy {listFromApi.Count} sản phẩm");

            //Chuyển về luồng UI để vẽ lên màn hình
            _dispatcher.TryEnqueue(() =>
            {
                Products.Clear();
                foreach (var item in listFromApi)
                {
                    Products.Add(item);
                }

                // Tắt loading cũng nên để trong này cho chắc (vì nó ảnh hưởng UI)
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
        // await _navigator.NavigateViewModelAsync<CartViewModel>(this);
    }

    // Lệnh mở trang Chi tiết
    [RelayCommand]
    private async Task GoToDetailAsync(Product selectedProduct)
    {
        if (selectedProduct == null) return;
        // await _navigator.NavigateViewModelAsync<ProductDetailViewModel>(this, data: selectedProduct);
    }
}
