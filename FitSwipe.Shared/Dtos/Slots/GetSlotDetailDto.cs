
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;

namespace FitSwipe.Shared.Dtos.Slots
{
    public class GetSlotDetailDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid? TrainingId { get; set; }
        public string CreateById { get; set; } = string.Empty;
        public SlotType Type { get; set; } //Phân biệt slot này là slot rảnh hay slot đã đặt
        public SlotStatus Status { get; set; } // Trạng thái bắt đầu hay kết thúc của 1 slot
        public PaymentStatus PaymentStatus { get; set; }
        public double? Rating { get; set; }
        public string? Feedback { get; set; }
        public GetUserDto CreateBy { get; set; } = default!;
        public GetTrainingDto Training { get; set; } = default!;
        //View properties
        public double TotalDurationInHour { 
            get => (EndTime - StartTime).TotalHours;
        }
        public int TotalPrice
        {
            get => (int)(TotalDurationInHour * Training.DealPrice);
        }
        public string TimeString
        {
            get => StartTime + " tới " + EndTime;
        }
        public string TotalDurationString
        {
            get => TotalDurationInHour.ToString("0.##") + " tiếng";
        }
        public string MoneyString
        {
            get => TotalPrice.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        }
        public string DealPriceString
        {
            get => Training.DealPrice.ToString("C0", new System.Globalization.CultureInfo("vi-VN")) + " /h";
        }
    }
}
