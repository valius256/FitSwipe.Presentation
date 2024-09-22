using FitSwipe.Shared.Enums;

namespace FitSwipe.Shared.Dtos.Users
{
    public class UpdateUserProfileDto
    {
        public Gender? SelectedGender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Job { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Bio { get; set; }
        public List<Guid> TagIds { get; set; } = new List<Guid>();
    }
}
