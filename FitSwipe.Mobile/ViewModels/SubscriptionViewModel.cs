using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.Pages.SubscriptionPages;
using FitSwipe.Shared.Dtos.Subscription;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class SubscriptionViewModel : ObservableObject
  {
    // Initialize the ObservableCollection
    public ObservableCollection<GetSubscriptionItemDto> Subscriptions { get; set; }

    public ObservableCollection<GetPaymentOptionDto> PaymentOptions { get; set; }

    [ObservableProperty]
    private GetSubscriptionItemDto selectedSubscription;

    [ObservableProperty]
    private GetSubscriptionItemDto currentSubscription;

    public RelayCommand NavigateToPaymentCommand { get; }

    [ObservableProperty]
    private GetPaymentOptionDto selectedPaymentOption;

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

      PaymentOptions = new ObservableCollection<GetPaymentOptionDto>
      {
        new GetPaymentOptionDto
        {
          Name = "FitSwipe",
          Description = "",
          Index = 1
        },
        new GetPaymentOptionDto
        {
          Name = "TPBank",
          Description = "Nhấn để thanh toán",
          Index = 2
        },
        new GetPaymentOptionDto
        {
          Name = "Momo",
          Description = "Nhấn để thanh toán",
          Index = 3
        }
      };

      // Set the initial subscriptions
      CurrentSubscription = Subscriptions[0];
      SelectedSubscription = Subscriptions[0];
      NavigateToPaymentCommand = new RelayCommand(async () => await NavigateToPaymentAsync());
    }

    public int CurrentSubscriptionIndex => Subscriptions.IndexOf(CurrentSubscription);

    partial void OnCurrentSubscriptionChanged (GetSubscriptionItemDto value)
    {
      // Update selected subscription when the current item changes
      SelectedSubscription = value;

      // Notify that the current index has changed
      OnPropertyChanged(nameof(CurrentSubscriptionIndex));
    }
    partial void OnSelectedPaymentOptionChanged (GetPaymentOptionDto value)
    {
      foreach (var option in PaymentOptions)
      {
        option.IsSelected = option == value; // Update selection state
      }
      OnPropertyChanged(nameof(PaymentOptions)); // Notify the UI of changes
    }

    private async Task NavigateToPaymentAsync ()
    {
      Debug.WriteLine($"Navigating to Payment with Subscription: {SelectedSubscription?.Name}");
      var paymentView = new SubscriptionPaymentView(SelectedSubscription, PaymentOptions);
      await Application.Current.MainPage.Navigation.PushAsync(paymentView);
    }
  }
}
