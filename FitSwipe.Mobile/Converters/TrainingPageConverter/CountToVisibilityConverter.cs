using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Mobile.Converters.TrainingPageConverter
{
  public class CountToVisibilityConverter : IValueConverter
  {
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is int count)
      {
        return count != 0; // Returns true if there are no items, thus making the label visible.
      }
      return true;
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
