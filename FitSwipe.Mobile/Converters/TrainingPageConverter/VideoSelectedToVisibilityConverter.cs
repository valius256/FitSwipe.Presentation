using FitSwipe.Shared.Dtos;
using System.Diagnostics;
using System.Globalization;

namespace FitSwipe.Mobile.Converters.TrainingPageConverter
{
  public class VideoSelectedToVisibilityConverter : IValueConverter
  {
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
    {
      var video = value as Video;
      var selectedVideo = parameter as Video;

      // Ensure the parameter is not null before comparison
      if (selectedVideo == null || video == null)
        return false;

      // Check if the current video is the selected one
      return video.Equals(selectedVideo); // or use reference equality if appropriate
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
