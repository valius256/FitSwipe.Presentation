
using FitSwipe.Shared.Dtos.Others;

namespace FitSwipe.Shared.Utils
{
    public static class Helper
    {
        public static GetWeekDto? GetCurrentWeek(List<GetWeekDto> weeks)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            return weeks.FirstOrDefault(week => today >= week.StartDate && today <= week.EndDate);
        }
        public static List<GetWeekDto> GetWeeksOfYear(int year)
        {
            var weeks = new List<GetWeekDto>();

            // Start from the first day of the year
            DateOnly firstDayOfYear = new DateOnly(year, 1, 1);

            // Find the first Monday of the year
            int offset = ((int)DayOfWeek.Monday - (int)firstDayOfYear.DayOfWeek + 7) % 7;
            DateOnly firstMonday = firstDayOfYear.AddDays(offset);

            DateOnly currentStartDate = firstMonday;

            while (currentStartDate.Year == year)
            {
                DateOnly currentEndDate = currentStartDate.AddDays(6);

                weeks.Add(new GetWeekDto
                {
                    StartDate = currentStartDate,
                    EndDate = currentEndDate
                });

                currentStartDate = currentStartDate.AddDays(7);
            }

            return weeks;
        }
    }


}
