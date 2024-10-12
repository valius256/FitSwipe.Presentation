
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;

namespace FitSwipe.Shared.Dtos.Trainings
{
    public class GetTrainingWithTraineeAndPTDto
    {
        public Guid Id { get; set; }
        public string TraineeId { get; set; } = string.Empty;
        public string PTId { get; set; } = string.Empty;
        public TrainingStatus Status { get; set; }
        public string? Feedback { get; set; }
        public double? Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? PricePerSlot { get; set; }
        public GetUserDto Trainee { get; set; } = default!;
        public GetUserDto PT { get; set; } = default!;
        public GetTrainingOverviewDto? TrainingOverview { get; set; } = new GetTrainingOverviewDto();

        //UI Props
        public string StatusString { get; set; } = string.Empty;
        public string StatusColor { get; set; } = string.Empty;
        public bool IsOffSchedule { get; set; } = false;


    }
}
