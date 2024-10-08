

using System.ComponentModel.DataAnnotations;

namespace FitSwipe.Shared.Dtos.Slots
{
    public class RequestCreateTrainingSlotDto
    {
        [Required]
        public required DateTime StartTime { get; set; }
        [Required]
        public required DateTime EndTime { get; set; }
        [Required]
        public required Guid BaseSlotId { get; set; } //Free slot in which trainee book upon
    }
}
