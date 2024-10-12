using FitSwipe.Mobile.Pages.PayingPages;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using Syncfusion.Maui.DataSource.Extensions;

namespace FitSwipe.Mobile.Pages.PayingPages;

public partial class PayingCheck : ContentPage
{
    public List<GetSlotDetailDto> Cart { get; set; } = new List<GetSlotDetailDto>();
    public int SelectedMethod { get; set; }
    private int _totalItems { get; set; } = 0;
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
    public PayingCheck()
    { 
        InitializeComponent();
        lblVnpay.TextColor = Colors.Black;
        lblWallet.TextColor = Colors.White;
        frameVnpay.BackgroundColor = Colors.Transparent;
        frameWallet.BackgroundColor = Color.FromArgb("#FF52BB00");
        FetchData();
        BindingContext = this;
    }

    private void FetchData()
    {
        TotalPrice = 0;
        TotalItems = 0;
        Cart = new List<GetSlotDetailDto>
        {
            new GetSlotDetailDto()
            {
                StartTime = new DateTime(2024,10,1,7,0,0),
                EndTime = new DateTime(2024,10,1,8,30,0),
                Training = new GetTrainingWithTraineeAndPTDto() {PricePerSlot = 150000},
                CreateBy = new GetUserDto() {UserName = "PT Trần A", AvatarUrl="pt1.png"},
            }, 
            new GetSlotDetailDto()
            {
                StartTime = new DateTime(2024,10,2,9,0,0),
                EndTime = new DateTime(2024,10,2,10,30,0),
                Training = new GetTrainingWithTraineeAndPTDto() {PricePerSlot = 150000},
                CreateBy = new GetUserDto() {UserName = "PT Trần A", AvatarUrl="pt1.png"},
            },
            new GetSlotDetailDto()
            {
                StartTime = new DateTime(2024,10,3,7,0,0),
                EndTime = new DateTime(2024,10,3,8,30,0),
                Training = new GetTrainingWithTraineeAndPTDto() {PricePerSlot = 150000},
                CreateBy = new GetUserDto() {UserName = "PT Trần A", AvatarUrl="pt1.png"},
            }
        };
        Cart.ForEach(s =>
        {
            TotalPrice += s.TotalPrice;
            TotalItems += 1;
        });
        TotalPriceString = TotalPrice.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
    }

    private async void btnPayment(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PayingView());
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
}
