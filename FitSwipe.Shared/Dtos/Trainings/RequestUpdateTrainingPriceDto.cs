
using System.ComponentModel.DataAnnotations;


namespace FitSwipe.Shared.Dtos.Trainings
{
    public class RequestUpdateTrainingPriceDto
    {
        public required Guid TrainingId { get; set; }
        public required int TrainingPrice { get; set; }
    }
}
