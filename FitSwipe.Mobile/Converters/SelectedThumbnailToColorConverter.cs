
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Enums;
using System.Globalization;

namespace FitSwipe.Mobile.Converters
{
    internal class SelectedThumbnailToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (parameter is string selectedThumbnail && value is string videoSource)
            {
                if (selectedThumbnail == videoSource)
                {
                    return "#52BB00";
                };
            }
            return "#AAAAAA";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
