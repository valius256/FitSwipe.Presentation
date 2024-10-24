using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages.SubscriptionPages;

public partial class SubscriptionView : ContentPage
{
    private SubscriptionViewModel viewModel;
    public SubscriptionView()
	{
		InitializeComponent();
        viewModel = new SubscriptionViewModel(loadingDialog);
        BindingContext = viewModel;
	}

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }

    private async void btnBuy_Clicked(object sender, EventArgs e)
    {
        await viewModel.NavigateToPayment();
    }
}