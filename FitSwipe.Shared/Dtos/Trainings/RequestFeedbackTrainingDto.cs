
namespace FitSwipe.Shared.Dtos.Trainings
{
    public class RequestFeedbackTrainingDto
    {
        public Guid TrainingId { get; set; }
        public string? Feedback { get; set; }
        public double? Rating { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}
