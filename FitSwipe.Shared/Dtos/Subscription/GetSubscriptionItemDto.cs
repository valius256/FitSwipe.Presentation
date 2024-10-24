using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Subscription
{
    public class GetSubscriptionItemDto : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Remaining time represented as TimeSpan
        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get => _remainingTime;
            set
            {
                if (_remainingTime != value)
                {
                    _remainingTime = value;
                    OnPropertyChanged(nameof(RemainingTime));
                    OnPropertyChanged(nameof(RemainingTimeSpan));
                }
            }
        }

        // Property to get a formatted remaining time string
        public string RemainingTimeSpan
        {
            get
            {
                // Check if RemainingTime is greater than zero
                if (RemainingTime > TimeSpan.Zero)
                {
                    return $"{RemainingTime.Days} ngày {RemainingTime.Hours} giờ {RemainingTime.Minutes} phút";
                }
                return "Đã hết hạn";
            }
        }

        public ObservableCollection<string> Benefit { get; set; } = [];
        public int Price { get; set; }

        public string PriceString
        {
            get
            {
                return $"{Price:N0} đ";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
