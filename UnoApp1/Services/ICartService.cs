using System;
using System.Collections.Generic;
using System.Text;
using UnoApp1.Models;
namespace UnoApp1.Services;

public interface ICartService
{
    // Lấy danh sách hàng trong giỏ
    Task<List<CartItem>> GetCartItemsAsync();

    // Thêm sản phẩm vào giỏ
    Task AddItemAsync(Product product, int quantity = 1);

    // Đếm tổng số lượng (để hiện lên icon giỏ hàng)
    Task<int> GetCartCountAsync();
    Task RemoveItemAsync(CartItem item);
    Task ClearCartAsync();
    Task UpdateItemAsync(CartItem item);
}
