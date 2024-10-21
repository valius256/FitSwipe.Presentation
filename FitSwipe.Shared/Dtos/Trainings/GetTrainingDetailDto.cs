using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Trainings
{
    public class GetTrainingDetailDto : INotifyPropertyChanged
    {
        public  Guid Id { get; set; }
        public string TraineeId { get; set; } = string.Empty;
        public string PTId { get; set; } = string.Empty;
        public TrainingStatus Status { get; set; }
        public string? Feedback { get; set; }
        public double? Rating { get; set; }
        public int? PricePerSlot { get; set; }

        public GetUserDto Trainee { get; set; } = default!;
        public GetUserDto PT { get; set; } = default!;
        public ObservableCollection<GetSlotDto> Slots { get; set; } = new ObservableCollection<GetSlotDto>();

        public int TotalHours { 
            get {
                double duration = 0;
                foreach (var slot in Slots)
                {
                    duration += (slot.EndTime - slot.StartTime).TotalHours;
                }
                return (int)duration;
            }
        }
        public DateTime? StartDate
        {
            get => Slots.OrderBy(s => s.StartTime).FirstOrDefault()?.StartTime;
        }
        public DateTime? EndDate
        {
            get => Slots.OrderBy(s => s.StartTime).LastOrDefault()?.EndTime;
        }

        public string DurationString
        {
            get
            {
                string start, end;
                if (StartDate.HasValue)
                {
                    start = StartDate.Value.Day + "/" + StartDate.Value.Month + "/" + StartDate.Value.Year;
                } else
                {
                    start = "Không xác định";
                }
                if (EndDate.HasValue)
                {
                    end = EndDate.Value.Day + "/" + EndDate.Value.Month + "/" + EndDate.Value.Year;
                }
                else
                {
                    end = "Không xác định";
                }
                return start + " - " + end;
            }
        }
        private string _statusString = string.Empty;
        public string StatusString {
            get => _statusString;
            set
            {
                if (_statusString != value)
                {
                    _statusString = value;
                    OnPropertyChanged(nameof(StatusString));
                }
            }
        }
        private string _statusColor = string.Empty;
        public string StatusColor
        {
            get => _statusColor;
            set
            {
                if (_statusColor != value)
                {
                    _statusColor = value;
                    OnPropertyChanged(nameof(StatusColor));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
