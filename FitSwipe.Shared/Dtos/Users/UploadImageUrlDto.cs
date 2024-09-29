

using System.ComponentModel.DataAnnotations;

namespace FitSwipe.Shared.Dtos.Users
{
    public class UpdateImageUrlDto
    {
        [Required]
        public required string Url { get; set; }
    }
}
