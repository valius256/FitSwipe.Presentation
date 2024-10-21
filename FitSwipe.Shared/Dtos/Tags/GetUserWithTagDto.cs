
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Tags
{
    public class GetUserWithTagDto : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string FireBaseId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string? Email { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string? Bio { get; set; }
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public Role Role { get; set; }
        public string Job { get; set; } = string.Empty;
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public UserStatus Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public RecordStatus RecordStatus { get; set; }
        //PT Exclusive
        public PTStatus? PTStatus { get; set; }
        public string? Description { get; set; }
        public double? PTExperienceYear { get; set; }
        public double? PTRating { get; set; }
        public ObservableCollection<GetTagDto> Tags { get; set; } = new ObservableCollection<GetTagDto>();
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
        public string AddressString
        {
            get => Ward + ", " + District + " ," + City;
        }

        private double _distanceInKm;
        public double DistanceInKm
        {
            get => _distanceInKm;
            set
            {
                if (_distanceInKm != value)
                {
                    _distanceInKm = value;
                    OnPropertyChanged(nameof(DistanceInKm));
                }
            }
        }
        private string _commonHobby = string.Empty;
        public string CommonHobby
        {
            get => _commonHobby;
            set
            {
                if (_commonHobby != value)
                {
                    _commonHobby = value;
                    OnPropertyChanged(nameof(CommonHobby));
                }
            }
        }
        private string _commonTrainingType = string.Empty;
        public string CommonTrainingType
        {
            get => _commonTrainingType;
            set
            {
                if (_commonTrainingType != value)
                {
                    _commonTrainingType = value;
                    OnPropertyChanged(nameof(CommonTrainingType));
                }
            }
        }
        private string _commonTarget = string.Empty;
        public string CommonTarget
        {
            get => _commonTarget;
            set
            {
                if (_commonTarget != value)
                {
                    _commonTarget = value;
                    OnPropertyChanged(nameof(CommonTarget));
                }
            }
        }
        private string _commonPTTaste = string.Empty;
        public string CommonPTTaste
        {
            get => _commonPTTaste;
            set
            {
                if (_commonTrainingType != value)
                {
                    _commonPTTaste = value;
                    OnPropertyChanged(nameof(CommonPTTaste));
                }
            }
        }
        private string _commonSelfDescription = string.Empty;
        public string CommonSelfDescription
        {
            get => _commonSelfDescription;
            set
            {
                if (_commonSelfDescription != value)
                {
                    _commonSelfDescription = value;
                    OnPropertyChanged(nameof(CommonSelfDescription));
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
