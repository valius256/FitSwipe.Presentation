using CommunityToolkit.Mvvm.ComponentModel;
using FitSwipe.Shared.Dtos.Subscription;
using System;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class SubscriptionViewModel : ObservableObject
  {
    // Initialize the ObservableCollection
    public ObservableCollection<GetSubscriptionItemDto> Subscriptions { get; set; }

    [ObservableProperty]
    private GetSubscriptionItemDto selectedSubscription;

    [ObservableProperty]
    private GetSubscriptionItemDto currentSubscription;

    public SubscriptionViewModel ()
    {
      // Initialize the collection in the constructor
      Subscriptions = new ObservableCollection<GetSubscriptionItemDto>
            {
                new GetSubscriptionItemDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Standard",
                    RemainingTime = new TimeSpan(days: 2, hours: 0, minutes: 0, seconds: 0),
                    Benefit = new ObservableCollection<string>
                    {
                        "Tăng khả năng tiếp cận học viên",
                        "Khung ảnh đại diện nổi bật",
                        "Đăng ảnh, video không giới hạn"
                    },
                    Price = 50000 // Corrected from 100.000 to 50000
                },
                new GetSubscriptionItemDto
                {
                    Id = Guid.NewGuid(),
                    Name = "VIP",
                    RemainingTime = new TimeSpan(days: 1, hours: 0, minutes: 0, seconds: 0),
                    Benefit = new ObservableCollection<string>
                    {
                        "Các đặc quyền của gói Standard",
                        "Tuỳ chỉnh profile cực đẹp",
                        "Khung chat nổi bật",
                    },
                    Price = 300000 // Corrected from 100.000 to 300000
                },
                new GetSubscriptionItemDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Super VIP",
                    RemainingTime = new TimeSpan(days: 1, hours: 18, minutes: 0, seconds: 0),
                    Benefit = new ObservableCollection<string>
                    {
                        "Các đặc quyền của VIP",
                        "Hầu hết học viên profile sẽ thấy profile của bạn",
                    },
                    Price = 500000 // Corrected from 100.000 to 500000
                }
            };

      // Set the initial subscriptions
      CurrentSubscription = Subscriptions[0];
      SelectedSubscription = Subscriptions[0];
    }

    public int CurrentSubscriptionIndex => Subscriptions.IndexOf(CurrentSubscription);

    partial void OnCurrentSubscriptionChanged (GetSubscriptionItemDto value)
    {
      // Update selected subscription when the current item changes
      SelectedSubscription = value;

      // Notify that the current index has changed
      OnPropertyChanged(nameof(CurrentSubscriptionIndex));
    }
  }
}
