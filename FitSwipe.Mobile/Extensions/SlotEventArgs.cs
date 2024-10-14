

using FitSwipe.Shared.Dtos.Slots;

namespace FitSwipe.Mobile.Extensions
{
    public class SlotEventArgs : EventArgs
    {
        public Border Border { get; set; }
        public GetSlotDto Slot { get; set; }

        public SlotEventArgs(Border border, GetSlotDto slot)
        {
            Border = border;
            Slot = slot;
        }
    }
}
