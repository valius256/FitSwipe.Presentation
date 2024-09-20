using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Mobile.Converters.SignUpConverter
{
    public class RoleToTextColorConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string role)
            {
                return role.Equals("pt", StringComparison.OrdinalIgnoreCase) ? Colors.White : Colors.Black;
            }
            return Colors.Black; // Default color
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
