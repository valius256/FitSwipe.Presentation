using System.Globalization;

namespace FitSwipe.Mobile.Converters.TrainingPageConverter
{
  class TabColorConverterMyPTList : IValueConverter
  {
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
    {
      int activeTab = (int)value;
      int tabIndex = int.Parse(parameter.ToString());

      return activeTab == tabIndex ? Color.FromArgb("#50B700") : Color.FromArgb("#C5C5C5");
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
