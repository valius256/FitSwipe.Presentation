using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages.SubscriptionPages;

public partial class SubscriptionView : ContentPage
{
	public SubscriptionView()
	{
		InitializeComponent();
		BindingContext = new SubscriptionViewModel();
	}
}