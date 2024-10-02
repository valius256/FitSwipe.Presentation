
using FitSwipe.Shared.Enums;
using System.Globalization;

namespace FitSwipe.Mobile.Converters
{
    public class EnumToBooleanConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is Gender gender && value is Gender selectedGender)
            {
                return gender == selectedGender;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is Gender gender)
            {
                return gender;
            }
            return Binding.DoNothing;
        }

    }
}
