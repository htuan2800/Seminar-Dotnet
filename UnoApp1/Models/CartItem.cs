using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
namespace UnoApp1.Models
{
    [Table("CartItems")]
    public partial class CartItem : ObservableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public decimal UnitPrice { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private int _quantity;

        // Thuộc tính này chỉ để hiển thị, không lưu vào DB
        [NotMapped]
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
