
namespace FitSwipe.Shared.Dtos.Medias
{
    public class RequestCreateUserMediaDto
    {
        public string MediaUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsVideo { get; set; } = false;
    }
}
