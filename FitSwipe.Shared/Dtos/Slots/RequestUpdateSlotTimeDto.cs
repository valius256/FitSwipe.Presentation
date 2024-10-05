
using System.ComponentModel.DataAnnotations;

namespace FitSwipe.Shared.Dtos.Slots
{
    public class RequestUpdateSlotTimeDto
    {
        [Required]
        public required Guid SlotId { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
