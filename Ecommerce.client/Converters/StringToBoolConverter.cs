using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Ecommerce.client.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        // Converts a non-empty string to true, otherwise false
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
                return !string.IsNullOrEmpty(str);
            return false;
        }

        // Not implemented, as it's not needed for one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
