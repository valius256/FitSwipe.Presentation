
namespace FitSwipe.Shared.Dtos.Others
{
    public class GetWeekDto
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Display => $"{StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}";

    }
}
