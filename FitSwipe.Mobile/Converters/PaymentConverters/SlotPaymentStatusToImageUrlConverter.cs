
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Enums;
using System.Globalization;

namespace FitSwipe.Mobile.Converters.PaymentConverters
{
    public class SlotPaymentStatusToImageUrlConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is GetSlotDto slot)
            {
                if (slot.Status != SlotStatus.Unbooked && slot.Status != SlotStatus.Pending && slot.Status != SlotStatus.Disabled)
                {
                    if (slot.PaymentStatus == PaymentStatus.NotPaid)
                    {
                        return "white-warning";
                    }
                    else if (slot.PaymentStatus == PaymentStatus.InDebt)
                    {
                        return "red-warning";
                    }
                }
                
            }
            return "";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
