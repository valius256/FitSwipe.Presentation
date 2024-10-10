using FitSwipe.Shared.Dtos;
using System;
using System.Globalization;

namespace FitSwipe.Mobile.Converters.TrainingPageConverter
{
  public class VideoSelectedToVisibilityConverter : IValueConverter
  {
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
    {
      // 'value' is the selected video (from the BindingContext)
      // 'parameter' is the current video (from the CollectionView)

      var selectedVideo = value as Video;
      var currentVideo = parameter as Video;

      // Ensure both videos are valid before comparison
      if (selectedVideo == null || currentVideo == null)
        return false;

      // Return true if the current video is the selected one
      return selectedVideo.Equals(currentVideo);
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
