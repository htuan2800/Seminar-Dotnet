using Microsoft.UI.Xaml.Data;
using System.Globalization;

namespace UnoApp1.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal price)
            {
                var culture = new CultureInfo("vi-VN");
                return price.ToString("N0", culture);
            }

            if (value is double priceDouble)
            {
                var culture = new CultureInfo("vi-VN");
                return priceDouble.ToString("N0", culture);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
