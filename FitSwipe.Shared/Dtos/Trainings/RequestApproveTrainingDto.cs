
namespace FitSwipe.Shared.Dtos.Trainings
{
    public class RequestApproveTrainingDto
    {
        public required Guid TrainingId { get; set; }
        public int? MinuteDistance { get; set; }
    }
}
