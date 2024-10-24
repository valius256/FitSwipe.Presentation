using FitSwipe.Mobile.Pages.PayingPages;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Payment;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using Syncfusion.Maui.DataSource.Extensions;
namespace FitSwipe.Mobile.Pages.PayingPages;

public partial class PayingCheck : ContentPage
{
    public List<GetSlotDetailDto> Cart { get; set; } = new List<GetSlotDetailDto>();
    public int SelectedMethod { get; set; }
    private int _totalItems { get; set; } = 0;

    private Func<Task>? _refresh;
    public int TotalItems {  
        get => _totalItems; 
        set { 
            _totalItems = value;
            OnPropertyChanged(nameof(TotalItems));
        }
    }
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
    private string _totalPriceString { get; set; } = "";
    public string TotalPriceString
    {
        get => _totalPriceString;
        set
        {
            _totalPriceString = value;
            OnPropertyChanged(nameof(TotalPriceString));
        }
    }
    public PayingCheck(List<GetSlotDetailDto> cart, Func<Task>? refresh = null)
    {
        InitializeComponent();
        lblVnpay.TextColor = Colors.Black;
        lblWallet.TextColor = Colors.White;
        frameVnpay.BackgroundColor = Colors.Transparent;
        frameWallet.BackgroundColor = Color.FromArgb("#FF52BB00");
        _refresh = refresh;

        Cart = cart;
        FetchData();
        _refresh = refresh;
    }

    private void FetchData()
    {
        TotalPrice = 0;
        TotalItems = 0;

        Cart.ForEach(s =>
        {
            TotalPrice += s.TotalPrice;
            TotalItems += 1;
        });
        TotalPriceString = TotalPrice.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        BindingContext = this;
    }

    private void tapWallet_Tapped(object sender, TappedEventArgs e)
    {
        SelectedMethod = 0;
        lblVnpay.TextColor = Colors.Black;
        lblWallet.TextColor = Colors.White;
        frameVnpay.BackgroundColor = Colors.Transparent;
        frameWallet.BackgroundColor = Color.FromArgb("#FF52BB00");
    }

    private void tapVnpay_Tapped(object sender, TappedEventArgs e)
    {
        SelectedMethod = 1;
        lblVnpay.TextColor = Colors.White;
        lblWallet.TextColor = Colors.Black;
        frameWallet.BackgroundColor = Colors.Transparent;
        frameVnpay.BackgroundColor = Color.FromArgb("#FF52BB00");
    }

    private async void btnPayment_Clicked(object sender, EventArgs e)
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
        } else
        {
            await HandleVnPay(token, currentUser);
        }
    }
    private async Task HandleBalance(string token)
    {

    }
    private async Task HandleVnPay(string token, GetUserDto currentUser)
    {
        var answer = await DisplayAlert("Xác nhận", "Bạn có chắc chắn muốn thanh toán bằng Payos?", "Có", "Không");
        if (answer)
        {
            loadingDialog.IsVisible = true;
            loadingDialog.Message = "Vui lòng chờ...";
            try
            {
                var result = await Fetcher.PostAsync<RequestPaySlotDto, GetPaymentUrlDto>("api/Payment/payos-create", new RequestPaySlotDto
                {
                    SlotIds = Cart.Select(s => s.Id.ToString()).ToList(),
                    OrderDescription = "Thanh toán buổi tập của " + currentUser.UserName + " vào lúc " + DateTime.Now,
                    ReturnUrl = "fitswipe://payment-completed"
                }, token);
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
            var queryString = "api/Slot?limit=100&Filter.PaymentStatus=" + PaymentStatus.Paid;
            foreach (var cart in Cart)
            {
                queryString += "&Filter.Ids=" + cart.Id;
            }
            var result = await Fetcher.GetAsync<PagedResult<GetSlotDto>>(queryString);
            if (result != null)
            {
                foreach (var item in Cart)
                {
                    if (!result.Items.Any(s => s.Id == item.Id))
                    {
                        await DisplayAlert("Thất bại", "Bạn chưa hoàn thành thanh toán hoặc giao dịch thất bại", "OK");
                        return;
                    }
                }
                //Fetch balance
                GetUserBalanceDto? balanceResult;
                try
                {
                    balanceResult = await Fetcher.GetAsync<GetUserBalanceDto>("api/users/balance", token);

                } catch
                {
                    balanceResult = null;
                }

                await Navigation.PushModalAsync(new PayingView(Cart,balanceResult?.Balance ?? -1 ,1, "SlotDetail", CloseFlag));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
        }
        loadingDialog.IsVisible = false;
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async Task CloseFlag()
    {
        if (_refresh != null)
        {
            await _refresh();
        }
        await Navigation.PopModalAsync();
    }
}
