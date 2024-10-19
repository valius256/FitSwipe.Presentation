
using FitSwipe.Shared.Dtos.FeedbackImages;
using FitSwipe.Shared.Dtos.Users;

namespace FitSwipe.Shared.Dtos.Trainings
{
    public class GetTrainingFeedbackDetailDto
    {
        public string? Feedback { get; set; }
        public double? Rating { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public GetUserDto Trainee { get; set; } = default!;
        public ICollection<GetFeedbackImageDto> FeedbackImages { get; set; } = new List<GetFeedbackImageDto>();
    }
}
