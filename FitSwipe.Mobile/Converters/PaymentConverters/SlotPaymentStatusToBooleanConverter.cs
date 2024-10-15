using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Enums;
using System.Globalization;

namespace FitSwipe.Mobile.Converters.PaymentConverters
{
    public class SlotPaymentStatusToBooleanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is GetSlotDto slot)
            {
                if (slot.PaymentStatus == PaymentStatus.Paid && slot.Status != SlotStatus.Unbooked && slot.Status != SlotStatus.Pending && slot.Status != SlotStatus.Disabled)
                {
                    return false;
                }
            }
            else if (value is GetSlotDetailDto slotDetail)
            {
                if (slotDetail.PaymentStatus == PaymentStatus.Paid && slotDetail.Status != SlotStatus.Unbooked && slotDetail.Status != SlotStatus.Pending && slotDetail.Status != SlotStatus.Disabled)
                {
                    return false;
                }
            }
            return true;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
