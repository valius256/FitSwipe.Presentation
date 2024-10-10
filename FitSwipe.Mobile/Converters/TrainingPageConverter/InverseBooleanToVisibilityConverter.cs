using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Mobile.Converters.TrainingPageConverter
{
  public class InverseBooleanToVisibilityConverter : IValueConverter
  {
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is bool booleanValue)
      {
        return !booleanValue; // Return the inverse of the boolean
      }
      return false; // Default to hidden if not a boolean
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
