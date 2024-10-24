using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Mobile.Pages.PayingPages;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Transactions;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Utils;
using Syncfusion.Maui.Core.Carousel;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.ProfilePages;

public partial class TraineePaymentPage : ContentPage
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
	private ObservableCollection<GetSlotDetailDto> _unpaidSlots = [];
	public ObservableCollection<GetSlotDetailDto> UnpaidSlots
	{
		get => _unpaidSlots;
		set
		{
			_unpaidSlots = value;
			OnPropertyChanged(nameof(UnpaidSlots));
		}
	}
    private ObservableCollection<GetSlotDetailDto> _aboutToPaidSlots = [];
    public ObservableCollection<GetSlotDetailDto> AboutToPaidSlots
    {
        get => _aboutToPaidSlots;
        set
        {
            _aboutToPaidSlots = value;
            OnPropertyChanged(nameof(AboutToPaidSlots));
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
	public TraineePaymentPage()
	{
		InitializeComponent();
		//Setup();
		BindingContext = this;
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
    public async void Setup()
	{
        await FetchUserData();
		await FetchUnpaidSlots();
        await FetchTransaction();
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

            } else
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
    public async Task FetchUnpaidSlots()
    {
        loadingDialog.IsVisible = true;
        try
        {
            AboutToPaidSlots.Clear();
            var userId = await SecureStorage.GetAsync("loginedUserId");
            if (userId == null)
            {
                throw new Exception("Lỗi xác thực");
            }
            var result = await Fetcher.GetAsync<List<GetSlotDetailDto>>($"api/Slot/debt-slots", _token);
            if (result != null)
            {
				UnpaidSlots = result.ToObservableCollection();
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
                    TotalPage = (int) Math.Ceiling((double) result.Total / pageSize); 
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
            transaction.AmountDisplay = transaction.Amount.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
            if (transaction.Status == Shared.Enum.TransactionStatus.Successed)
            {
                transaction.TextColor = "#52BB00";
            } else
            {
                transaction.TextColor = "Red";
            }

            if (transaction.Type == Shared.Enum.TransactionType.Deposit)
            {
                transaction.AmountColor = "#52BB00";
                transaction.AmountDisplay = "+ " + transaction.AmountDisplay;
            }
            else if (transaction.Type == Shared.Enum.TransactionType.Withdrawal)
            {
                transaction.AmountColor = "Gray";
                transaction.AmountDisplay = "- " + transaction.AmountDisplay;
            }
            else if (transaction.Type == Shared.Enum.TransactionType.AutoDeduction)
            {
                transaction.AmountColor = "Gray";
                transaction.AmountDisplay = "- " + transaction.AmountDisplay;
            }
            else if (transaction.Type == Shared.Enum.TransactionType.DirectPayment)
            {
                transaction.AmountColor = "#0394fc";
            }
        }
    }
    private async void btnWithdraw_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//WithdrawRequestPage?isTrainee=true");
    }

    private void btnDeposit_Clicked(object sender, EventArgs e)
    {
        rechargeModal.Setup("#52BB00",FetchUserData);
        rechargeModal.Show();
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // Get the CheckBox that triggered the event
        var checkbox = (CheckBox)sender;

        // Get the Week object from the BindingContext
        var slot = (GetSlotDetailDto)checkbox.BindingContext;
        if (slot != null)
        {
            if (e.Value)
            {
                AboutToPaidSlots.Add(slot);
            }
            else
            {
                AboutToPaidSlots.Remove(slot);
            }
        }
    }


    private async void btnPayWallet_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Xác nhận", "Bạn có chắc chắn muốn thanh toán những buổi đã chọn hay không?", "Có", "Không");
        if (answer)
        {
            await Navigation.PushModalAsync(new PayingCheck(AboutToPaidSlots.ToList(), FetchUnpaidSlots));
        }
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
}