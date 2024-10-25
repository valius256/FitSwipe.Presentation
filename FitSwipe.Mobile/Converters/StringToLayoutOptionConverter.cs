

using System.Globalization;

namespace FitSwipe.Mobile.Converters
{
    internal class StringToLayoutOptionConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return stringValue.ToLower() switch
                {
                    "start" => LayoutOptions.Start,
                    "center" => LayoutOptions.Center,
                    "end" => LayoutOptions.End,
                    "fill" => LayoutOptions.Fill,
                    _ => LayoutOptions.Fill
                };
            }
            return LayoutOptions.Fill;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
