using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.Controls;
using FitSwipe.Mobile.Pages.SubscriptionPages;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Payment;
using FitSwipe.Shared.Dtos.Subscription;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Utils;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace FitSwipe.Mobile.ViewModels
{
    public partial class SubscriptionViewModel : ObservableObject
    {
        private LoadingDialog _loadingDialog;
        // Initialize the ObservableCollection
        public ObservableCollection<GetSubscriptionItemDto> Subscriptions { get; set; }

        [ObservableProperty]
        private GetSubscriptionItemDto currentSubscription;

        [ObservableProperty]
        private bool isHavingSubscriptions = false;

        //public int CurrentSubscriptionIndex => Subscriptions.IndexOf(CurrentSubscription);
        //public RelayCommand NavigateToPaymentCommand { get; }
        //public RelayCommand NavigateToPaymentResultCommand { get; }
        //public ICommand SelectPaymentOptionCommand { get; }

        public SubscriptionViewModel(LoadingDialog loadingDialog)
        {
            _loadingDialog = loadingDialog;
            // Initialize the collection in the constructor
            Subscriptions = new ObservableCollection<GetSubscriptionItemDto>
            {
                new GetSubscriptionItemDto
                {
                    Id = Guid.NewGuid(),
                    Name = "VIP 1",
                    RemainingTime = new TimeSpan(days: 2, hours: 0, minutes: 0, seconds: 0),
                    Benefit = new ObservableCollection<SubscriptionBenefit>
                    {
                        new SubscriptionBenefit{ BenefitContent = "Tăng khả năng tiếp cận học viên" },
                        new SubscriptionBenefit{ BenefitContent = "Khung ảnh đại diện nổi bật" },
                        new SubscriptionBenefit{ BenefitContent = "Đăng ảnh, video không giới hạn số lượng" }
                    },
                    Price = 99000 // Corrected from 100.000 to 50000
                },
                //new GetSubscriptionItemDto
                //{
                //    Id = Guid.NewGuid(),
                //    Name = "VIP",
                //    RemainingTime = new TimeSpan(days: 1, hours: 0, minutes: 0, seconds: 0),
                //    Benefit = new ObservableCollection<string>
                //    {
                //        "Các đặc quyền của gói Standard",
                //        "Tuỳ chỉnh profile cực đẹp",
                //        "Khung chat nổi bật",
                //    },
                //    Price = 300000 // Corrected from 100.000 to 300000
                //},
                //new GetSubscriptionItemDto
                //{
                //    Id = Guid.NewGuid(),
                //    Name = "Super VIP",
                //    RemainingTime = new TimeSpan(days: 1, hours: 18, minutes: 0, seconds: 0),
                //    Benefit = new ObservableCollection<string>
                //    {
                //        "Các đặc quyền của VIP",
                //        "Hầu hết học viên profile sẽ thấy profile của bạn",
                //    },
                //    Price = 500000 // Corrected from 100.000 to 500000
                //}
            };



            // Set the initial subscriptions
            CurrentSubscription = Subscriptions[0];
            //NavigateToPaymentCommand = new RelayCommand(async () => await NavigateToPaymentAsync());
            //NavigateToPaymentResultCommand = new RelayCommand(async () => await NavigateToPaymentResultAsync());
            //SelectPaymentOptionCommand = new RelayCommand<GetPaymentOptionDto>(SelectPaymentOption);
            FetchCurrentSubscriptions();
        }
        public async void FetchCurrentSubscriptions()
        {
            _loadingDialog.IsVisible = true;
            _loadingDialog.Message = "Đang lấy dữ liệu...";
            var token = await SecureStorage.GetAsync("auth_token");
            if (token != null)
            {
                try
                {
                    var result = await Fetcher.GetAsync<GetUserSubscriptionDto>("api/users/subscriptions", token);
                    if (result == null)
                    {
                        throw new Exception("Không thể lấy dữ liệu");
                    }
                    if (result.SubscriptionLevel != null && result.SubscriptionLevel > 0 && result.SubscriptionPurchasedDate != null)
                    {
                        CurrentSubscription = Subscriptions[result.SubscriptionLevel.Value - 1];
                        IsHavingSubscriptions = true;
                        CurrentSubscription.RemainingTime = result.SubscriptionPurchasedDate.Value.AddDays(30) - DateTime.UtcNow.AddHours(7);

                    }
                }
                catch (Exception ex)
                {
                    if (Application.Current != null && Application.Current.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                    }
                }
            }
            _loadingDialog.IsVisible = false;
        }

        public async Task NavigateToPayment()
        {
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new SubscriptionPaymentView(currentSubscription));
            }
            
        }
    }
}
