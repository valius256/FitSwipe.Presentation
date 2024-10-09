using System;
using System.Collections.ObjectModel;

namespace FitSwipe.Shared.Dtos.Subscription
{
  public class GetSubscriptionItemDto
  {
    public Guid Id { get; set; }
    public string Name { get; set; }

    // Remaining time represented as TimeSpan
    public TimeSpan RemainingTime { get; set; }

    // Property to get a formatted remaining time string
    public string RemainingTimeSpan
    {
      get
      {
        // Check if RemainingTime is greater than zero
        if (RemainingTime > TimeSpan.Zero)
        {
          return $"{RemainingTime.Days} ngày {RemainingTime.Hours} giờ {RemainingTime.Minutes} phút {RemainingTime.Seconds} giây";
        }
        return "No remaining time";
      }
    }

    public ObservableCollection<string> Benefit { get; set; }
    public double Price { get; set; }

    public string PriceString
    {
      get
      {
        return $"{Price:N0} đ";
      }
    }
  }
}
