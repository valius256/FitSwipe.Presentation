
namespace FitSwipe.Shared.Dtos.Tags
{
    public class RequestUpsertTagsDto
    {
        //public string UserId { get; set; } = string.Empty;// Remove later
        public List<Guid> NewTagIds { get; set; } = [];
    }
}
