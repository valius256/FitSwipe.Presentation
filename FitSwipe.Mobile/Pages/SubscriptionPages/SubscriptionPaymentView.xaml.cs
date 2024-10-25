using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Payment;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Subscription;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FitSwipe.Mobile.Pages.SubscriptionPages;

public partial class SubscriptionPaymentView : ContentPage
{
    public DateTime Now { get; set; } = DateTime.Now;

    private int _balance = 0;
    public int Balance
    {
        get => _balance;
        set
        {
            _balance = value;
            OnPropertyChanged(nameof(Balance));
        }
    }
    public GetUserSubscriptionDto OriginalSubscription = new();
    private GetSubscriptionItemDto selectedSubscription = new();
    public GetSubscriptionItemDto SelectedSubscription
    {
        get => selectedSubscription;
        set
        {
            selectedSubscription = value;
            OnPropertyChanged(nameof(SelectedSubscription));
        }
    }
    //public ObservableCollection<GetPaymentOptionDto> PaymentOptions { get; set; }
    public int SelectedMethod { get; set; } = 0;
    private int _totalPrice { get; set; } = 0;
    public int TotalPrice
    {
        get => _totalPrice;
        set
        {
            _totalPrice = value;
            OnPropertyChanged(nameof(TotalPrice));
        }
    }
    public SubscriptionPaymentView(GetSubscriptionItemDto selectedSubscription)
    {
        InitializeComponent();
        TotalPrice = selectedSubscription.Price;
        SelectedSubscription = selectedSubscription;

        Setup();
        BindingContext = this;
    }
    private async void Setup()
    {
        await FetchOldSubscription();
        await FetchBalance();
    }
    private void tapPayos_Tapped(object sender, TappedEventArgs e)
    {
        SelectedMethod = 1;
        lblPayos.TextColor = Colors.White;
        lblWallet.TextColor = Colors.Black;
        frameWallet.BackgroundColor = Colors.White;
        framePayos.BackgroundColor = Color.FromArgb("#FF323593");
    }
    private async Task FetchBalance()
    {
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Vui lòng chờ...";
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token == null)
            {
                throw new Exception();
            }
            var balanceResult = await Fetcher.GetAsync<GetUserBalanceDto>("api/users/balance", token);
            if (balanceResult == null)
            {
                throw new Exception();
            }
            Balance = balanceResult.Balance;
        }
        catch (Exception)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra khi lấy thống tin số dư. Bạn nên thoát trang và mở lại", "OK");
            await Navigation.PopModalAsync();
        }
        loadingDialog.IsVisible = false;
    }
    private async Task FetchOldSubscription()
    {
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Vui lòng chờ...";
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token == null)
            {
                throw new Exception();
            }
            var result = await Fetcher.GetAsync<GetUserSubscriptionDto>("api/users/subscriptions", token);
            if (result == null)
            {
                throw new Exception();
            }
            OriginalSubscription = result;
        }
        catch (Exception)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra khi lấy thống tin gói đăng ký cũ. Bạn nên thoát trang và mở lại", "OK");
            await Navigation.PopModalAsync();
        }
        loadingDialog.IsVisible = false;       
    }
    private async Task HandleBalance(string token)
    {
        var answer = await DisplayAlert("Xác nhận", "Bạn có chắc chắn muốn thanh toán bằng số dư?", "Có", "Không");
        if (answer)
        {
            loadingDialog.IsVisible = true;
            loadingDialog.Message = "Vui lòng chờ...";
            try
            {
                if (Balance < selectedSubscription.Price)
                {
                    throw new Exception("Không đủ số dư");
                }
                int level = selectedSubscription.Name switch
                {
                    "VIP 1" => 1,
                    _ => 1,
                };
                 await Fetcher.PutAsync("api/Payment/balance-payment-subscription?level=" + level, new BlankDto { }, token);
                await HandleAfterPayment(token);             
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
            }
            loadingDialog.IsVisible = false;
        }
    }
    private async void btnPay_Clicked(object sender, EventArgs e)
    {
        //AUTHENTICATION
        string? token;
        GetUserDto? currentUser;
        try
        {
            token = await SecureStorage.GetAsync("auth_token");
            if (token == null)
            {
                throw new Exception();
            }
            currentUser = await Shortcut.GetLoginedUser(token);
            if (currentUser == null)
            {
                throw new Exception();
            }
        }
        catch
        {
            await DisplayAlert("Lỗi", "Lỗi xác thực. Vui lòng đăng nhập lại", "OK");
            await Shell.Current.GoToAsync("//SignIn");
            return;
        }
        //HANDLE DIFFERENT METHOD
        if (SelectedMethod == 0)
        {
            await HandleBalance(token);
        }
        else
        {
            await HandlePayos(token, currentUser);
        }
    }
    private async Task HandlePayos(string token, GetUserDto currentUser)
    {
        var answer = await DisplayAlert("Xác nhận", "Bạn có chắc chắn muốn thanh toán bằng Payos?", "Có", "Không");
        if (answer)
        {
            loadingDialog.IsVisible = true;
            loadingDialog.Message = "Vui lòng chờ...";
            try
            {
                int level = selectedSubscription.Name switch
                {
                    "VIP 1" => 1,
                    _ => 1,
                };
                var result = await Fetcher.PostAsync<BlankDto, GetPaymentUrlDto>("api/Payment/payos-create-subscription?level=" + level, new BlankDto {}, token);
                if (result != null)
                {
                    await Browser.Default.OpenAsync(result.Url, BrowserLaunchMode.SystemPreferred);

                    await Task.Delay(2000);
                    await DisplayAlert("Xác nhận hoàn tất thanh toán", "Bạn đã thanh toán trên Payos", "Kiểm tra");
                    await HandleAfterPayment(token);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
            }
            loadingDialog.IsVisible = false;
        }
    }

    private async Task HandleAfterPayment(string token)
    {
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Vui lòng chờ...";
        try
        {
            var result = await Fetcher.GetAsync<GetUserSubscriptionDto>("api/users/subscriptions", token);
            if (result == null)
            {
                throw new Exception();
            }
            
            if (result.SubscriptionPurchasedDate != OriginalSubscription.SubscriptionPurchasedDate)
            {
                //SUCCESS
                await Navigation.PushModalAsync(new SubscriptionResultView(new GetPaymentResultDto
                {
                    paymentDate = DateTime.Now,
                    Method = "Payos",
                    subscriptionItem = SelectedSubscription,
                    BalanceLeftString = $"{Balance:N0} đ",
                }));
            } else
            {
                throw new Exception("Giao dịch thất bại hoặc bạn chưa hoàn tất thanh toán");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
        }
        loadingDialog.IsVisible = false;
    }

    private void tapWallet_Tapped(object sender, TappedEventArgs e)
    {
        SelectedMethod = 0;
        lblPayos.TextColor = Colors.Black;
        lblWallet.TextColor = Colors.White;
        framePayos.BackgroundColor = Colors.White;
        frameWallet.BackgroundColor = Color.FromArgb("#FF323593");
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}