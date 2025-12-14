using System;
using System.Collections.Generic;
using System.Text;
using UnoApp1.Data;
using Microsoft.EntityFrameworkCore;
namespace UnoApp1.Services;

public class CartService : ICartService
{
    // Hàm thêm vào giỏ
    public async Task AddItemAsync(Product product, int quantity = 1)
    {
        using var db = new AppDbContext();

        var existingItem = await db.CartItems
            .FirstOrDefaultAsync(x => x.ProductId == product.Id);

        if (existingItem != null)
        {
            // Tăng theo số lượng được chọn
            existingItem.Quantity += quantity;
        }
        else
        {
            var newItem = new CartItem
            {
                ProductId = product.Id,
                ProductName = product.product_name,
                ProductImageUrl = product.imageUrl,
                UnitPrice = product.price,
                Quantity = quantity // ⭐ Sử dụng quantity
            };
            db.CartItems.Add(newItem);
        }

        await db.SaveChangesAsync();
    }

    public async Task<List<CartItem>> GetCartItemsAsync()
    {
        using var db = new AppDbContext();
        return await db.CartItems.ToListAsync();
    }

    public async Task<int> GetCartCountAsync()
    {
        using var db = new AppDbContext();
        // Tính tổng số lượng (Quantity) của tất cả các món
        return await db.CartItems.SumAsync(x => x.Quantity);
    }

    public async Task RemoveItemAsync(CartItem item)
    {
        using var db = new AppDbContext();
        // Tìm món đó trong DB
        var itemToDelete = await db.CartItems.FirstOrDefaultAsync(x => x.Id == item.Id);
        if (itemToDelete != null)
        {
            db.CartItems.Remove(itemToDelete);
            await db.SaveChangesAsync();
        }
    }

    public async Task UpdateItemAsync(CartItem item)
    {
        using var db = new AppDbContext();

        // Tìm món hàng trong DB
        var existingItem = await db.CartItems.FirstOrDefaultAsync(x => x.Id == item.Id);

        if (existingItem != null)
        {
            // Cập nhật số lượng mới
            existingItem.Quantity = item.Quantity;

            // Nếu số lượng <= 0 thì xóa luôn
            if (existingItem.Quantity <= 0)
            {
                db.CartItems.Remove(existingItem);
            }

            await db.SaveChangesAsync();
        }
    }

    // Hàm Xóa sạch giỏ hàng (Dùng cho mục 3.6)
    public async Task ClearCartAsync()
    {
        using var db = new AppDbContext();
        // Xóa toàn bộ bảng CartItems
        db.CartItems.RemoveRange(db.CartItems);
        await db.SaveChangesAsync();
    }
}
