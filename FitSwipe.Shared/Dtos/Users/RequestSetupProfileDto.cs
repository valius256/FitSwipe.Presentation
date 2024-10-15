

using FitSwipe.Shared.Enums;
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Users
{
    public class RequestSetupProfileDto : INotifyPropertyChanged
    {
        public string? UserName {  get; set; }
        public Gender? Gender { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? Job { get; set; }
        private string? _city { get; set; }

        public string? City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged(nameof(City));
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
