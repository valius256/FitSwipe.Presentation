using FitSwipe.Mobile.Pages.TrainingPages;
using FitSwipe.Shared.Dtos.Slots;

namespace FitSwipe.Mobile.Pages.PayingPages;

public partial class PayingView : ContentPage
{
    public List<GetSlotDetailDto> Cart { get; set; } = new List<GetSlotDetailDto>();
    public string BalanceFormat { get; set; }
    public string TotalPriceFormat { get; set; }
    public string Method { get; set; }
    public DateTime Time { get; set; }
    private string _prePage {  get; set; }
    private Func<Task>? _closeFlag;
    public PayingView(List<GetSlotDetailDto> cart, int balance, int method, string prePageName, Func<Task>? closeFlag = null)
    {
        InitializeComponent();

        _prePage = prePageName;
        _closeFlag = closeFlag;

        Cart = cart;
        Time = DateTime.Now;
        Method = method == 0 ? "Trừ số dư ví FitSwipe" : "VnPay";
        if (balance >= 0)
        {
            BalanceFormat = balance.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        }
        else
        {
            BalanceFormat = "Lấy dữ liệu số dư thất bại";
        }

        var total = 0;
        foreach (var item in Cart)
        {
            total += item.TotalPrice;
        }
        TotalPriceFormat = total.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));

        BindingContext = this;
    }

    private async void btnCumBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();

        if (_prePage == "SlotDetail")
        {
            if (_closeFlag != null)
            {
                await _closeFlag();
            }
        }    
    }
}