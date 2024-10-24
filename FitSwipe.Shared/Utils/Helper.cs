
using FitSwipe.Shared.Dtos.Others;
using FitSwipe.Shared.Dtos.Slots;

namespace FitSwipe.Shared.Utils
{
    public static class Helper
    {
        public static string? LoginedUserId { get; set; }

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
        /// <summary>
        /// Calculate distance between 2 geolocation
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in kilometers

            // Convert degrees to radians
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            // Apply the Haversine formula
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Distance in kilometers
            return R * c;
        }

        public static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static bool CheckTokenChanged(string token1, string token2)
        {
            if (token1 == null || token2 == null)
            {
                return true;
            }
            if (token1 != token2)
            {
                return true;
            }
            return false;
        }
    }


}
