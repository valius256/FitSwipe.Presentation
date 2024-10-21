
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Slots
{
    public class GetSlotDetailDto : INotifyPropertyChanged
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

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

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
            get => StartTime.ToString("hh:mm") + " tới " + EndTime.ToString("hh:mm");
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
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
