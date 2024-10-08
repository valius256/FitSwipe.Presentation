using FitSwipe.Shared.Dtos.Tags;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Mobile.Converters.ProfilePageConverters
{
    public class TagTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GetTagDto tag && parameter is string tagType)
            {
                return tag.TagType.ToString() == tagType ? tag.Name : string.Empty; // Return null instead of an empty string
            }

            return null; // Return null for any non-matching values
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
