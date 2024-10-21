

namespace FitSwipe.Shared.Dtos.Medias
{
    public class RequestUpdateUserMediaDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
