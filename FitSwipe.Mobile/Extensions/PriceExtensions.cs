

namespace FitSwipe.Mobile.Extensions
{
    public static class PriceExtensions
    {
        public static string ToVND(this int currString)
        {
            return currString.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        }
    }
}
