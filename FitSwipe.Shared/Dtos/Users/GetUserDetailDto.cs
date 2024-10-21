
using FitSwipe.Shared.Dtos.Medias;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Users
{
    public class GetUserDetailDto : INotifyPropertyChanged
    {
        public string FireBaseId { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string Phone { get; set; } = string.Empty;
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public Role Role { get; set; }
        public double PricePerHour { get; set; }
        public UserStatus Status { get; set; }
        public int? Balance { get; set; } = 0;
        //PT Exclusive
        public PTStatus? PTStatus { get; set; }
        public string? Description { get; set; }
        public double? PTExperienceYear { get; set; }
        public double? PTRating { get; set; }
        public string? PTDegreeImageUrl { get; set; }
        public DateTime? SubscriptionPurchasedDate { get; set; }
        public int? SubscriptionLevel { get; set; }
        public PaymentStatus? SubscriptionPaymentStatus { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public RecordStatus RecordStatus { get; set; }
        //Navigator

        public ICollection<GetTrainingDto> TrainingsInstructing { get; set; } = new List<GetTrainingDto>();
        public ICollection<GetTrainingDto> TrainingsAttending { get; set; } = new List<GetTrainingDto>();
        public ICollection<GetSlotDto> SlotsCreated { get; set; } = new List<GetSlotDto>();
        //Observabke Property
        public string _userName = string.Empty;
        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }
        public string? _avatarUrl;
        public string? AvatarUrl
        {
            get => _avatarUrl;
            set
            {
                if (_avatarUrl != value)
                {
                    _avatarUrl = value;
                    OnPropertyChanged(nameof(AvatarUrl));
                }
            }
        }
        public string? _job;
        public string? Job
        {
            get => _job;
            set
            {
                if (_job != value)
                {
                    _job = value;
                    OnPropertyChanged(nameof(Job));
                }
            }
        }
        public string? _city;
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
        public string? _bio;
        public string? Bio
        {
            get => _bio;
            set
            {
                if (_bio != value)
                {
                    _bio = value;
                    OnPropertyChanged(nameof(Bio));
                }
            }
        }
        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (_dateOfBirth != value)
                {
                    _dateOfBirth = value;
                    OnPropertyChanged(nameof(DateOfBirth));
                    OnPropertyChanged(nameof(Age));
                }
            }
        }
        public int Age => DateTime.Now.Year - DateOfBirth.Year;


        private ObservableCollection<GetTagDto> _tags = new ObservableCollection<GetTagDto>();
        public ObservableCollection<GetTagDto> Tags
        {
            get => _tags;
            set
            {
                if (_tags != value)
                {
                    _tags = value;
                    OnPropertyChanged(nameof(Tags));
                }
            }
        }

        private ObservableCollection<GetUserMediaDto> _userMedias { get; set; } = new ObservableCollection<GetUserMediaDto>();
        public ObservableCollection<GetUserMediaDto> UserMedias
        {
            get => _userMedias;
            set
            {
                if (_userMedias != value)
                {
                    _userMedias = value;
                    OnPropertyChanged(nameof(UserMedias));
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
