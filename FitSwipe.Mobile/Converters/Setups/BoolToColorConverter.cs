

using System.Globalization;

namespace FitSwipe.Mobile.Converters.Setups
{
    public class BoolToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is bool isSelected && values[1] is string mainColor)
            {
                return isSelected ? Color.Parse(mainColor) : Colors.Gray; // Adjust as needed
            }
            return Colors.Gray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
