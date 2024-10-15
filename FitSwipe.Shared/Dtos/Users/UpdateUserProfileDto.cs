using FitSwipe.Shared.Enums;
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Users
{
    public class UpdateUserProfileDto :INotifyPropertyChanged
    {
        public Gender? SelectedGender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Job { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? Bio { get; set; }
        public Role Role { get; set; }
        public List<Guid> TagIds { get; set; } = new List<Guid>();
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
