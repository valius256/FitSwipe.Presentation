
using FitSwipe.Shared.Dtos.Others;
using FitSwipe.Shared.Dtos.Slots;

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

        public static bool IsConflict(DateTime startTime, DateTime endTime, ICollection<GetSlotDto> slots, GetSlotDto? slotToSkip = null)
        {
            foreach (var slot in slots)
            {
                if (slotToSkip != null && slotToSkip.Id == slot.Id) continue;
                if (startTime <= slot.EndTime && endTime >= slot.StartTime)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// return 00:00:00 of the monday of this week
        /// </summary>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek()
        {
            DateTime today = DateTime.Today;
            int daysSinceMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;

            // Adjust for when today is Sunday, so we correctly set the first day to Monday
            if (daysSinceMonday < 0)
            {
                daysSinceMonday += 7;
            }

            DateTime firstDayOfWeek = today.AddDays(-daysSinceMonday);
            return firstDayOfWeek;
        }
        /// <summary>
        /// return 23:59:59 of the sunday of this week
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeek()
        {
            DateTime today = DateTime.Today;
            int daysUntilSunday = (int)DayOfWeek.Sunday - (int)today.DayOfWeek;

            // Adjust for when today is Sunday, so the last day should be today
            if (daysUntilSunday < 0)
            {
                daysUntilSunday += 7;
            }
            DateTime lastDayOfWeek = today.AddDays(daysUntilSunday);
            return lastDayOfWeek.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

    }


}
