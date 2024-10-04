using FitSwipe.Shared.Enums;

using System.ComponentModel.DataAnnotations;

namespace FitSwipe.Shared.Dtos.Trainings
{
    public class CreateTrainingDto
    {
        [Required]
        public required string PTId { get; set; }
        public TrainingStatus Status { get; set; } = TrainingStatus.Matched;
    }
}
