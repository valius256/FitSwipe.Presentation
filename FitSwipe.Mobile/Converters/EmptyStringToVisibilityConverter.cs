
using System.Globalization;

namespace FitSwipe.Mobile.Converters
{
    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string stringVal)
            {
                if (!string.IsNullOrEmpty(stringVal))
                {
                    return true;
                }
            }
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
