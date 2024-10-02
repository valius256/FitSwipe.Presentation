
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;

namespace FitSwipe.Shared.Dtos.Tags
{
    public class GetUserWithTagDto
    {
        public Guid Id { get; set; }
        public string? FireBaseId { get; set; }
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
    }
}
