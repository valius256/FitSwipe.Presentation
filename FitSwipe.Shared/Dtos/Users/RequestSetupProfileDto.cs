

using FitSwipe.Shared.Enums;

namespace FitSwipe.Shared.Dtos.Users
{
    public class RequestSetupProfileDto
    {
        public Gender? Gender { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Street { get; set; }
        public string? Job { get; set; }
        
    }
}
