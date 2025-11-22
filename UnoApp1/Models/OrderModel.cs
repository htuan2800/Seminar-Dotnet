using System;
using System.Collections.Generic;
using System.Text;

namespace UnoApp1.Models;
public class OrderModel
{
    public string CustomerName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    // Tổng tiền đơn hàng
    public decimal GrandTotal { get; set; }

    // Danh sách các món hàng họ mua
    public List<CartItem> Items { get; set; } = new List<CartItem>();
}
