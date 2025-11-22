using System;
using System.Collections.Generic;
using System.Text;

namespace UnoApp1.Models;
public class Product
{
    // ID của sản phẩm từ Server trả về
    public string Id { get; set; }

    public string product_name { get; set; }

    public string unit { get; set; }

    public DateTimeOffset created_at { get; set; }

    public decimal price { get; set; }

    public string imageUrl { get; set; }

    public string barcode { get; set; }
}
