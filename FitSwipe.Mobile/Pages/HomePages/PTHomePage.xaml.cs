using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Mobile.Pages.SubscriptionPages;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Subscription;
using FitSwipe.Shared.Dtos.Transactions;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Utils;
using FitSwipe.Mobile.Extensions;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.HomePages;

public partial class PTHomePage : ContentPage
{
    private string _token = string.Empty;
    private int pageSize = 10;

    private bool _isRefresh =false;
    public bool IsRefresh
    {
        get => _isRefresh;
        set
        {
            _isRefresh = value;
            OnPropertyChanged(nameof(IsRefresh));
        }
    }
    private bool _isHavingSubscriptions = false;
    public bool IsHavingSubscriptions
    {
        get => _isHavingSubscriptions;
        set
        {
            _isHavingSubscriptions = value;
            OnPropertyChanged(nameof(IsHavingSubscriptions));
        }
    }
    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }
    private int _totalPage = -1;
    public int TotalPage
    {
        get => _totalPage;
        set
        {
            _totalPage = value;
            OnPropertyChanged(nameof(TotalPage));
        }
    }
    private string balance = string.Empty;
    public string Balance
    {
        get => balance;
        set
        {
            balance = value;
            OnPropertyChanged(nameof(Balance));
        }
    }
    private GetSubscriptionItemDto? subscription;
    public GetSubscriptionItemDto? Subscription
    {
        get => subscription;
        set
        {
            subscription = value;
            OnPropertyChanged(nameof(Subscription));
        }
    }
    private GetUserDto _userDto = new();
    public GetUserDto User
    {
        get => _userDto;
        set
        {
            _userDto = value;
            OnPropertyChanged(nameof(User));
        }
    }
    private ObservableCollection<GetSlotDetailDto> _upcomingSlots = [];
    public ObservableCollection<GetSlotDetailDto> UpcomingSlots
    {
        get => _upcomingSlots;
        set
        {
            _upcomingSlots = value;
            OnPropertyChanged(nameof(UpcomingSlots));
        }
    }
    private ObservableCollection<GetTransactionDto> _transactions = [];
    public ObservableCollection<GetTransactionDto> Transactions
    {
        get => _transactions;
        set
        {
            _transactions = value;
            OnPropertyChanged(nameof(Transactions));
        }
    }
    public List<GetSubscriptionItemDto> Subscriptions { get; set; }


    public PTHomePage()
    {
        InitializeComponent();
        //Setup();
        Subscriptions = new List<GetSubscriptionItemDto>
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
        };
        BindingContext = this;
    }
    public async void Setup()
    {
        await FetchUserData();
        await FetchUpcomingSlots();
        await FetchTransaction();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var currentToken = await SecureStorage.GetAsync("auth_token") ?? string.Empty;
        if (Helper.CheckTokenChanged(_token, currentToken))
        {
            _token = currentToken;
            Setup();
            return;
        }
    }
    public async Task FetchUserData()
    {
        pageContent.IsVisible = false;
        loadingDialog.IsVisible = true;
        try
        {
            var user = await Shortcut.GetLoginedUser(_token);
            if (user == null)
            {
                throw new Exception("Lỗi xác thực");
            }
            User = user;
            var balanceData = await Fetcher.GetAsync<GetUserBalanceDto>("api/users/balance", _token);
            if (balanceData != null)
            {
                Balance = balanceData.Balance.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));

            }
            else
            {
                Balance = "Lấy số dư thất bại";
            }
            var subscriptionData = await Fetcher.GetAsync<GetUserSubscriptionDto>("api/users/subscriptions", _token);
            if (subscriptionData != null && subscriptionData.SubscriptionPurchasedDate != null && subscriptionData.SubscriptionLevel != null)
            {
                IsHavingSubscriptions = true;
                Subscription = new GetSubscriptionItemDto
                {
                    Name = "VIP " + subscriptionData.SubscriptionLevel,
                    RemainingTime = subscriptionData.SubscriptionPurchasedDate.Value.AddDays(30) - DateTime.Now,
                    Benefit = Subscriptions[subscriptionData.SubscriptionLevel.Value - 1].Benefit
                };

            } else
            {
                IsHavingSubscriptions = false;
                Subscription = null;
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
            await Shell.Current.GoToAsync("//SignIn");
        }

        pageContent.IsVisible = true;
        loadingDialog.IsVisible = false;
    }
    public async Task FetchUpcomingSlots()
    {
        loadingDialog.IsVisible = true;
        try
        {
            var userId = await SecureStorage.GetAsync("loginedUserId");
            if (userId == null)
            {
                throw new Exception("Lỗi xác thực");
            }
            var result = await Fetcher.GetAsync<List<GetSlotDetailDto>>($"api/Slot/upcoming-slots?limit=3", _token);
            if (result != null)
            {
                UpcomingSlots = result.ToObservableCollection();
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
        }

        loadingDialog.IsVisible = false;
    }
    public async Task FetchTransaction()
    {
        loadingDialog.IsVisible = true;
        try
        {
            var userId = await SecureStorage.GetAsync("loginedUserId");
            if (userId == null)
            {
                throw new Exception("Lỗi xác thực");
            }
            var result = await Fetcher.GetAsync<PagedResult<GetTransactionDto>>($"api/Payment/transactions?Filter.Status=1&Filter.Status=2&Filter.Status=3&page={CurrentPage}&limit={pageSize}", _token);
            if (result != null)
            {
                Transactions = result.Items.ToObservableCollection();
                if (TotalPage <= 0)
                {
                    TotalPage = (int)Math.Ceiling((double)result.Total / pageSize);
                }
                FormatTransactions();
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
        }

        loadingDialog.IsVisible = false;
    }

    private void FormatTransactions()
    {
        foreach (var transaction in Transactions)
        {
            if (transaction.Status == Shared.Enum.TransactionStatus.Successed)
            {
                transaction.TextColor = "#52BB00";
            }
            else
            {
                transaction.TextColor = "Red";
            }

            if (transaction.Type == Shared.Enum.TransactionType.Deposit)
            {
                transaction.AmountColor = "#52BB00";
                transaction.AmountDisplay = "+ " + transaction.Amount.ToVND();
            }
            else if (transaction.Type == Shared.Enum.TransactionType.AutoDeduction || transaction.Type == Shared.Enum.TransactionType.DirectPayment)
            {
                transaction.AmountColor = "#52BB00";
                transaction.AmountDisplay = "+ " + (transaction.Amount * 97 / 100).ToVND();
                transaction.CommissionFee = "(Đã trừ " + (transaction.Amount * 3 / 100).ToVND() + " phí hoa hồng)";
            }
            else if (transaction.Type == Shared.Enum.TransactionType.Withdrawal)
            {
                transaction.AmountColor = "Gray";
                transaction.AmountDisplay = "- " + transaction.Amount.ToVND();
            }
            else
            {
                transaction.AmountColor = "#0394fc";
                transaction.AmountDisplay = transaction.Amount.ToVND();
            }
        }
    }
    private async void btnWithdraw_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//WithdrawRequestPage?isTrainee=false");
    }

    private void btnDeposit_Clicked(object sender, EventArgs e)
    {
        rechargeModal.Setup("#52BB00", FetchUserData);
        rechargeModal.Show();
    }


    private async void btnNext_Clicked(object sender, EventArgs e)
    {
        if (CurrentPage < TotalPage)
        {
            CurrentPage += 1;
            await FetchTransaction();
        }
    }

    private async void btnPrev_Clicked(object sender, EventArgs e)
    {
        if (CurrentPage > 1)
        {
            CurrentPage -= 1;
            await FetchTransaction();
        }
    }

    private async void btnSchedule_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//PTSchedulePage");
    }

    private void pageContent_Refreshing(object sender, EventArgs e)
    {
        IsRefresh = false;
        Setup();
    }

    private void btnSubscription_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new SubscriptionView());
    }
}