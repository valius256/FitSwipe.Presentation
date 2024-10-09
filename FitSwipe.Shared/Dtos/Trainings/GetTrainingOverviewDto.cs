
namespace FitSwipe.Shared.Dtos.Trainings
{
    public class GetTrainingOverviewDto
    {
        public DateTime? StartTime { get; set; } //Start time of the first slot
        public DateTime? EndTime { get; set; } //End time of the last slot
        public int TotalDuration { get; set; } //Total duration in hours of all slots
        public int TotalSlots { get; set; } //Total slot numbers

        //UI PROPS
        public string SlotString => TotalSlots + " buổi, " + TotalDuration + " tiếng";
        public string DurationString
        {
            get
            {
                string start, end;
                if (StartTime.HasValue)
                {
                    start = StartTime.Value.Day + "/" + StartTime.Value.Month + "/" + StartTime.Value.Year;
                }
                else
                {
                    start = "Không xác định";
                }
                if (EndTime.HasValue)
                {
                    end = EndTime.Value.Day + "/" + EndTime.Value.Month + "/" + EndTime.Value.Year;
                }
                else
                {
                    end = "Không xác định";
                }
                return start + " - " + end;
            }
        }
    }
}
