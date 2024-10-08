using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Mobile.Converters.ProfilePageConverters
{
    public class UserTagColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is a string and not null or empty
            if (value is string colorString && !string.IsNullOrWhiteSpace(colorString))
            {
                // Return the Color from the hex string
                return Color.FromArgb(colorString);
            }

            // Return a default color (optional)
            return Colors.Orange; // You can choose another color if you want
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
