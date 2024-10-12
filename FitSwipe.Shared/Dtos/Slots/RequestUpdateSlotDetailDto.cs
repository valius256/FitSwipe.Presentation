

using System.ComponentModel.DataAnnotations;

namespace FitSwipe.Shared.Dtos.Slots
{
    public class RequestUpdateSlotDetailDto
    {
        public required Guid SlotId { get; set; }
        public List<RequestCreateSlotVideoDto> SlotVideos { get; set; } = [];
        public string? Location { get; set; }
    }
}
