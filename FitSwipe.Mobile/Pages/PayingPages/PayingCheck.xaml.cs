using FitSwipe.Mobile.Pages.PayingPages;

namespace FitSwipe.Mobile.Pages.PayingPages;

public partial class PayingCheck : ContentPage
{
    public PayingCheck()
    {
        InitializeComponent();
    }

    private async void btnPayment(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PayingView());
    }

}
