using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UnoApp1.Models;
internal class CartItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Lưu ID của sản phẩm để sau này biết là mua cái gì
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public string ProductImageUrl { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }


    [NotMapped]
    public decimal TotalPrice => UnitPrice * Quantity;
}
