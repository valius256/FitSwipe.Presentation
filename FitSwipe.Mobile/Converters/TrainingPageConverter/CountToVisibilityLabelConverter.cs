using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Mobile.Converters.TrainingPageConverter
{
  public class CountToVisibilityLabelConverter : IValueConverter
  {
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
    {
      // Check if the value is an integer and cast it
      if (value is int count)
      {
        // If count is 0, return true (label should be visible)
        return count == 0;
      }
      // If the value is not an integer, return false (default to hidden)
      return false;
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
