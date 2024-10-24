using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Dtos.Payment;

namespace FitSwipe.Mobile.Pages.SubscriptionPages;

public partial class SubscriptionResultView : ContentPage
{
    private GetPaymentResultDto _paymentResultDto = new();
    public GetPaymentResultDto PaymentResultDto
    {
        get => _paymentResultDto;
        set
        {
            _paymentResultDto = value;
            OnPropertyChanged(nameof(PaymentResultDto));
        }
    }
    public SubscriptionResultView(GetPaymentResultDto getPaymentResultDto)
	{
        PaymentResultDto = getPaymentResultDto;
		InitializeComponent();
        BindingContext = this;
        
	}

    private async void back_Clicked(object sender, EventArgs e)
    {
        bool isPlayAnimation = true;
        while (Shell.Current.Navigation.ModalStack.Count > 0)
        {
            await Shell.Current.Navigation.PopModalAsync(isPlayAnimation); // Set false to prevent animation
            isPlayAnimation = false;
        }
    }
}