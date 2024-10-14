

namespace FitSwipe.Mobile.Extensions
{
    public class BookSlotEventArgs : EventArgs
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan TimeBegin { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public Guid? BaseSlotId { get; set; }
        public Guid? EditSlotId { get; set; }
        public BookSlotEventArgs(DateTime start, DateTime end, TimeSpan timeBegin, TimeSpan timeEnd, Guid? baseSlotId, Guid? editSlotId = null)
        {
            Start = start;
            End = end;
            TimeBegin = timeBegin;
            TimeEnd = timeEnd;
            BaseSlotId = baseSlotId;
            EditSlotId = editSlotId;
        }
    }
}
