using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Medias;
using FitSwipe.Shared.Dtos.Payment;
using FitSwipe.Shared.Utils;

namespace FitSwipe.Mobile.Pages.ProfilePages;

public partial class RechargeModal : ContentView
{
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
    private int _amount = 0;
    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            OnPropertyChanged(nameof(Amount));
        }
    }
    private Func<Task>? _refresh;
    public RechargeModal()
	{
		InitializeComponent();
        Hide();
        BindingContext = this;
        
	}
    public void Setup(string theme, Func<Task> refresh)
    {
        Theme = theme;
        _refresh = refresh;
        UpdateButtons();
    }
    public void Show()
    {
        IsVisible = true;
        InputTransparent = false;
    }

    public void Hide()
    {
        IsVisible = false;
        InputTransparent = true;

    }
    private async void btnConfirm_Clicked(object sender, EventArgs e)
    {
        if (Application.Current != null && Application.Current.MainPage != null && !loadingDialog.IsVisible)
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Nạp tiền", "Bạn có chắc chắn về hành động này không?", "Có", "Không");
            if (answer)
            {
                
                try
                {
                    if (Amount < 10000)
                    {
                        throw new Exception("Vui lòng nạp tối thiểu 50.000đ");
                    }
                    loadingDialog.IsVisible = true;
                    var token = await SecureStorage.GetAsync("auth_token");
                    if (token == null)
                    {
                        throw new Exception("Lỗi xác thực");
                    }
                    var result = await Fetcher.PostAsync<BlankDto, GetPaymentUrlDto>("api/Payment/payos-create-deposit?amount=" + Amount, new BlankDto(), token);
                    if (result != null)
                    {
                        await Browser.Default.OpenAsync(result.Url, BrowserLaunchMode.SystemPreferred);

                        await Task.Delay(2000);
                        await Application.Current.MainPage.DisplayAlert("Xác nhận hoàn tất thanh toán", "Bạn đã thanh toán trên Payos?", "Kiểm tra");
                        //await HandleAfterPayment(token);
                        if (_refresh != null)
                        {
                            await _refresh();
                        }
                        Hide();
                    }

                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                }
                loadingDialog.IsVisible = false;


            }
        }
    }

    private void btnClose_Clicked(object sender, EventArgs e)
    {
        Hide();
    }
    private void UpdateButtons()
    {
        price1.BackgroundColor = Color.Parse(Amount == 50000 ? Theme : "#FFFFFF");
        price1_label.TextColor = Color.Parse(Amount == 50000 ? "#FFFFFF" : Theme);
        price2.BackgroundColor = Color.Parse(Amount == 100000 ? Theme : "#FFFFFF");
        price2_label.TextColor = Color.Parse(Amount == 100000 ? "#FFFFFF" : Theme);
        price3.BackgroundColor = Color.Parse(Amount == 200000 ? Theme : "#FFFFFF");
        price3_label.TextColor = Color.Parse(Amount == 200000 ? "#FFFFFF" : Theme);
        price4.BackgroundColor = Color.Parse(Amount == 500000 ? Theme : "#FFFFFF");
        price4_label.TextColor = Color.Parse(Amount == 500000 ? "#FFFFFF" : Theme);
        price5.BackgroundColor = Color.Parse(Amount == 1000000 ? Theme : "#FFFFFF");
        price5_label.TextColor = Color.Parse(Amount == 1000000 ? "#FFFFFF" : Theme);
        price6.BackgroundColor = Color.Parse(Amount == 2000000 ? Theme : "#FFFFFF");
        price6_label.TextColor = Color.Parse(Amount == 2000000 ? "#FFFFFF" : Theme);
    }
    private void tapPrice1_Tapped(object sender, TappedEventArgs e)
    {
        Amount = 50000;
        UpdateButtons();
    }

    private void tapPrice2_Tapped(object sender, TappedEventArgs e)
    {
        Amount = 100000;
        UpdateButtons();
    }

    private void tapPrice3_Tapped(object sender, TappedEventArgs e)
    {
        Amount = 200000;
        UpdateButtons();
    }

    private void tapPrice4_Tapped(object sender, TappedEventArgs e)
    {
        Amount = 500000;
        UpdateButtons();
    }

    private void tapPrice5_Tapped(object sender, TappedEventArgs e)
    {
        Amount = 1000000;
        UpdateButtons();
    }

    private void tapPrice6_Tapped(object sender, TappedEventArgs e)
    {
        Amount = 2000000;
        UpdateButtons();
    }
}