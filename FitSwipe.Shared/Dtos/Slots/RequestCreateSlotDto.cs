
using System.ComponentModel.DataAnnotations;

namespace FitSwipe.Shared.Dtos.Slots
{
    public class RequestCreateSlotDto
    {
        [Required]
        public required DateTime StartTime { get; set; }
        [Required]
        public required DateTime EndTime { get; set; }
    }
}
