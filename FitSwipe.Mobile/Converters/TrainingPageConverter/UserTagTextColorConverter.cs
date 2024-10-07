using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Mobile.Converters.TrainingPageConverter
{
  public class UserTagTextColorConverter : IValueConverter
  {
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
    {
      // Check if the value is a string and not null or empty
      if (value is string colorString && !string.IsNullOrWhiteSpace(colorString))
      {
        // Create a color from the hex string
        var color = Color.FromArgb(colorString);

        // Check brightness
        if (IsBrightColor(color))
        {
          return Colors.Black; // Return black if the color is too bright
        }

        return color; // Return the original color
      }

      // Return a default color (optional)
      return Colors.Orange; // You can choose another color if you want
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    // Method to determine if a color is bright
    private bool IsBrightColor (Color color)
    {
      // Calculate brightness using luminance formula
      // Using the formula: 0.299 * R + 0.587 * G + 0.114 * B
      double brightness = 0.299 * color.Red + 0.587 * color.Green + 0.114 * color.Blue;

      // Consider a threshold; you can adjust the threshold as needed
      return brightness > 0.7; // This means if brightness is above 0.7, consider it bright
    }
  }
}
