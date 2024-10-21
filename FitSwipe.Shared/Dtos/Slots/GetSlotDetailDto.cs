
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;

namespace FitSwipe.Shared.Dtos.Slots
{
    public class GetSlotDetailDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid? TrainingId { get; set; }
        public string CreateById { get; set; } = string.Empty;
        public SlotType Type { get; set; } //Phân biệt slot này là slot rảnh hay slot đã đặt
        public SlotStatus Status { get; set; } // Trạng thái bắt đầu hay kết thúc của 1 slot
        public PaymentStatus PaymentStatus { get; set; }
        public double? Rating { get; set; }
        public string? Feedback { get; set; }
        public string? Location { get; set; }
        public string? Content { get; set; }
        public GetUserDto CreateBy { get; set; } = default!;
        public GetTrainingWithTraineeAndPTDto Training { get; set; } = default!;
        public ObservableCollection<GetSlotVideoDto> Videos { get; set; } = new ObservableCollection<GetSlotVideoDto>();

        //View properties
        public double TotalDurationInHour { 
            get => (EndTime - StartTime).TotalHours;
        }
        public int TotalPrice
        {
            get => Training.PricePerSlot ?? 0;
        }
        public string TimeString
        {
            get => StartTime.ToString("hh:mm tt") + " tới " + EndTime.ToString("hh:mm tt");
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
            get => (Training.PricePerSlot ?? 0).ToString("C0", new System.Globalization.CultureInfo("vi-VN")) + " / buổi";
        }
    }
}
