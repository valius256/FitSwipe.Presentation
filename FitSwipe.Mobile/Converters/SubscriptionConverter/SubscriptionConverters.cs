using System.Globalization;

namespace FitSwipe.Mobile.Converters.SubscriptionConverter
{
  public class SelectedBackgroundColorConverter : IValueConverter
  {
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (bool)value ? Color.FromHex("#323593") : Colors.Transparent; // Change as needed
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class SelectedTextColorConverter : IValueConverter
  {
    public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (bool)value ? Colors.White : Colors.Black; // Change as needed
    }

    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  public class SelectedBorderColorConverter : IValueConverter
  {
    public object? Convert (object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      return (bool)value ? Colors.Transparent : Color.FromHex("#323593"); // Change as needed
    }

    public object? ConvertBack (object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
  public class SelectedBorderThicknessConverter : IValueConverter
  {
    public object? Convert (object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      return (bool)value ? 1 : 2;
    }

    public object? ConvertBack (object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
