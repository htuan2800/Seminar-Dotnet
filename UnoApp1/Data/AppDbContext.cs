using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace UnoApp1.Data;
internal class AppDbContext : DbContext
{
    public DbSet<CartItem> CartItems { get; set; }

    // Constructor này cực quan trọng!
    [RequiresUnreferencedCode("")]
    public AppDbContext()
    {
        //this.Database.EnsureDeleted();
        //nếu chưa có file .db thì nó TỰ TẠO.
        this.Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // --- ĐÂY LÀ PHẦN LOGIC ĐƯỜNG DẪN MÀ ĐỀ BÀI YÊU CẦU ---

        string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "shop.db");

        // Cấu hình SQLite sử dụng đường dẫn vừa tìm được
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}
