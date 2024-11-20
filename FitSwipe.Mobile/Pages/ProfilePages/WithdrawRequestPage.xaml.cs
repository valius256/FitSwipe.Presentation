using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.RequestWithdraw;
using FitSwipe.Shared.Dtos.Transactions;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Utils;
using Syncfusion.Maui.Core.Carousel;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.ProfilePages;

[QueryProperty(nameof(PassedIsTrainee), "isTrainee")]
public partial class WithdrawRequestPage : ContentPage
{
    private string _token = string.Empty;
    private int pageSize = 10;

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
    private int balanceNumber = 0;
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
    private bool isTrainee = false;
    public bool IsTrainee
    {
        get => isTrainee;
        set
        {
            isTrainee = value;
            OnPropertyChanged(nameof(IsTrainee));
        }
    }
    private bool isRefreshing = false;
    public bool IsRefreshing
    {
        get => isRefreshing;
        set
        {
            isRefreshing = value;
            OnPropertyChanged(nameof(IsRefreshing));
        }
    }
    public bool PassedIsTrainee { get; set; }

    private bool isCreating = false;
    public bool IsCreating
    {
        get => isCreating;
        set
        {
            isCreating = value;
            OnPropertyChanged(nameof(IsCreating));
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
    private string _theme = "#52BB00";
    public string Theme
    {
        get => _theme;
        set
        {
            _theme = value;
            OnPropertyChanged(nameof(Theme));
        }
    }
    private RequestCreateRequestWithdrawDto _createRequest = new();
    public RequestCreateRequestWithdrawDto CreateRequest
    {
        get => _createRequest;
        set
        {
            _createRequest = value;
            OnPropertyChanged(nameof(CreateRequest));
        }
    }
    private ObservableCollection<GetRequestWithdrawDto> _requests = [];
    public ObservableCollection<GetRequestWithdrawDto> Requests
    {
        get => _requests;
        set
        {
            _requests = value;
            OnPropertyChanged(nameof(Requests));
        }
    }
    private ObservableCollection<string> _bankNames = [];
    public ObservableCollection<string> BankNames
    {
        get => _bankNames;
        set
        {
            _bankNames = value;
            OnPropertyChanged(nameof(BankNames));
        }
    }
    public WithdrawRequestPage()
	{
		InitializeComponent();
        Setup();
        BankNames = new ObservableCollection<string>
        {
            "Momo","VPBank","Techcombank","BIDV","Vietcombank","VietinBank","MBBank","ACB","SHB","VIB","HDBank","SeABank","TPBank","LPBank","OCB","SCB","MSB","Sacombank","Eximbank","Nam A Bank","ABBANK","PVCombank","Bac A Bank","VietBank","NCB","BVBank","Viet A Bank","DongA Bank","PGBank","Kienlongbank","Saigonbank","Baoviet Bank"
        };
        BindingContext = this;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(10);
        IsTrainee = PassedIsTrainee;

        navbar.IsVisible = IsTrainee;
        navbarPT.IsVisible = !IsTrainee;
        profileNavbar.IsVisible = IsTrainee;
        profileNavbarPT.IsVisible = !IsTrainee;

        var currentToken = await SecureStorage.GetAsync("auth_token") ?? string.Empty;
        
        if (Helper.CheckTokenChanged(_token, currentToken))
        {
            _token = currentToken;
            Setup();
            return;
        }

    }
    public async void Setup()
    {
        await FetchUserData();
        await FetchRequests();

        IsTrainee = PassedIsTrainee;

    }
    public async Task FetchUserData()
    {
        pageContent.IsVisible = false;
        loadingDialog.IsVisible = true;
        try
        {
            if (string.IsNullOrEmpty(_token))
            {
                _token = await SecureStorage.GetAsync("auth_token") ?? string.Empty;
            }
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
                balanceNumber = balanceData.Balance;

            }
            else
            {
                Balance = "Lấy số dư thất bại";
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

    public async Task FetchRequests()
    {
        loadingDialog.IsVisible = true;
        try
        {
            var userId = await SecureStorage.GetAsync("loginedUserId");
            if (userId == null)
            {
                throw new Exception("Lỗi xác thực");
            }
            var result = await Fetcher.GetAsync<PagedResult<GetRequestWithdrawDto>>($"api/Payment/withdraw-user?page={CurrentPage}&limit={pageSize}", _token);
            if (result != null)
            {
                Requests = result.Items.ToObservableCollection();
                if (TotalPage <= 0)
                {
                    TotalPage = (int)Math.Ceiling((double)result.Total / pageSize);
                }
                FormatRequests();
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
        }

        loadingDialog.IsVisible = false;
    }
    private void FormatRequests()
    {
        foreach (var request in Requests)
        {
            if (request.Status == Shared.Enums.RequestStatus.Pending)
            {
                request.StatusBackground = "Gray";
                request.StatusDisplay = "Đang xử lý";
            }
            else if (request.Status == Shared.Enums.RequestStatus.Approved)
            {
                request.StatusBackground = "Green";
                request.StatusDisplay = "Thành công";
            }
            else
            {
                request.StatusBackground = "Red";
                request.StatusDisplay = "Thất bại";
            }
        }
    }

    private void btnWithdraw_Clicked(object sender, EventArgs e)
    {
        IsCreating = true;
    }

    private async void btnNext_Clicked(object sender, EventArgs e)
    {
        if (CurrentPage < TotalPage)
        {
            CurrentPage += 1;
            await FetchRequests();
        }
    }

    private async void btnPrev_Clicked(object sender, EventArgs e)
    {
        if (CurrentPage > 1)
        {
            CurrentPage -= 1;
            await FetchRequests();
        }
    }

    private void btnCancelCreate_Clicked(object sender, EventArgs e)
    {
        IsCreating = false;
    }

    private async void btnCreate_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Xác nhận rút tiền", "Sau khi bạn tạo đơn bạn sẽ bị trừ tiền và không thể hoàn tác. Bạn có chắc về hành động này chứ?", "Tôi đã hiểu", "Quay lại");
        if (answer) { 
            loadingDialog.IsVisible = true;
            try
            {
                if (balanceNumber < CreateRequest.Amount)
                {
                    throw new Exception("Không đủ số dư");
                }
                await Fetcher.PostAsync("api/Payment/withdraw", CreateRequest, _token);

                IsCreating = false;
                await DisplayAlert("Thành công", "Thành công. Đã tạo đơn thành công. Chúng tôi sẽ chuyển khoản lại bạn trong 24h", "OK");
                await FetchRequests();
                await FetchUserData();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
            }

        }
        

        loadingDialog.IsVisible = false;
    }

    private async void pageContent_Refreshing(object sender, EventArgs e)
    {
        if (!loadingDialog.IsVisible)
        {
            IsRefreshing = false;
            await FetchRequests();
            await FetchUserData();
        }
    }
}