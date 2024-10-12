
using FitSwipe.Shared.Enums;

namespace FitSwipe.Shared.Dtos.Slots
{
    public class GetSlotDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid? TrainingId { get; set; }
        public string CreateById { get; set; } = string.Empty;
        public SlotType Type { get; set; }
        public SlotStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? Location { get; set; }
        public string? Content { get; set; }
        public double? Rating { get; set; }
        public string? Feedback { get; set; }
        public string? Color { get; set; }
        public int TotalVideo { get; set; }
        public string TimeString
        {
            get => StartTime.TimeOfDay + " tới " + EndTime.TimeOfDay;
        }
        public string DateString => $"{StartTime:dd/MM/yyyy}";
        public int SlotNumber {  get; set; }
    }
}
