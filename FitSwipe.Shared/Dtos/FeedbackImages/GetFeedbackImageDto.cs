
namespace FitSwipe.Shared.Dtos.FeedbackImages
{
    public class GetFeedbackImageDto
    {
        public string MediaUrl { get; set; } = string.Empty;
        public Guid TrainingId { get; set; }
    }
}
