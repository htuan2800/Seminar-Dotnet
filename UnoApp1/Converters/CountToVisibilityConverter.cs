using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.UI.Xaml.Data;

namespace UnoApp1.Converters
{
    public class CountToVisibilityConverter : IValueConverter
    {
        // Hàm chuyển đổi từ Dữ liệu (int) -> Giao diện (Visibility)
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Kiểm tra xem giá trị truyền vào có phải là số không
            if (value is int count)
            {
                // Nếu số lượng > 0 thì HIỆN (Visible), ngược lại thì ẨN (Collapsed)
                return count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            // Mặc định là Ẩn
            return Visibility.Collapsed;
        }

        // Hàm chuyển ngược (không dùng trong trường hợp này)
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
