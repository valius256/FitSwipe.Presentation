
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;

namespace FitSwipe.Shared.Dtos.Trainings
{
    public class GetTrainingWithTraineeAndPT
    {
        public Guid Id { get; set; }
        public string TraineeId { get; set; } = string.Empty;
        public string PTId { get; set; } = string.Empty;
        public TrainingStatus Status { get; set; }
        public string? Feedback { get; set; }
        public double? Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public GetUserDto Trainee { get; set; } = default!;
        public GetUserDto PT { get; set; } = default!;

        //public int TotalHours
        //{
        //    get
        //    {
        //        double duration = 0;
        //        foreach (var slot in Slots)
        //        {
        //            duration += (slot.EndTime - slot.StartTime).TotalHours;
        //        }
        //        return (int)duration;
        //    }
        //}
        //public DateTime? StartDate
        //{
        //    get => Slots.First()?.StartTime;
        //}
        //public DateTime? EndDate
        //{
        //    get => Slots.Last()?.EndTime;
        //}
        //public string DurationString
        //{
        //    get
        //    {
        //        string start, end;
        //        if (StartDate.HasValue)
        //        {
        //            start = StartDate.Value.Day + "/" + StartDate.Value.Month + "/" + StartDate.Value.Year;
        //        }
        //        else
        //        {
        //            start = "Không xác định";
        //        }
        //        if (EndDate.HasValue)
        //        {
        //            end = EndDate.Value.Day + "/" + EndDate.Value.Month + "/" + EndDate.Value.Year;
        //        }
        //        else
        //        {
        //            end = "Không xác định";
        //        }
        //        return start + " - " + end;
        //    }
        //}

    }
}
