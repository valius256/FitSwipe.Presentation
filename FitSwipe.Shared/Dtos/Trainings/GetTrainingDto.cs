using FitSwipe.Shared.Enums;

namespace FitSwipe.Shared.Dtos.Trainings
{
    public class GetTrainingDto
    {
        public string TraineeId { get; set; } = string.Empty;
        public string PTId { get; set; } = string.Empty;
        public TrainingStatus Status { get; set; }
        public string? Feedback { get; set; }
        public double? Rating { get; set; }
        public int DealPrice { get; set; }
    }
}
