
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Tags;
using System.Globalization;

namespace FitSwipe.Mobile.Converters.ProfilePageConverters
{
    public class TagTypeToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is GetTagDto tag && parameter is string tagType){
                if (tag.TagType.ToString() == tagType) return true;
            }
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
