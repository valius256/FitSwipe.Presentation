using System.Globalization;

namespace FitSwipe.Mobile.Converters.TrainingPageConverter
{
    public class TabColorConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            int activeTab = (int)value;
            int tabIndex = int.Parse(parameter.ToString());

            return activeTab == tabIndex ? Color.FromArgb("#2E3191")  : Color.FromArgb("#C5C5C5");
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
