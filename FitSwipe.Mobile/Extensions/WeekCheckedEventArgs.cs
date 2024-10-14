using FitSwipe.Shared.Dtos.Others;

namespace FitSwipe.Mobile.Extensions
{
    public class WeekCheckedEventArgs : EventArgs
    {
        public List<GetWeekDto> Weeks { get; set; }

        public WeekCheckedEventArgs(List<GetWeekDto> weeks)
        {
            Weeks = weeks;
        }
    }
}
