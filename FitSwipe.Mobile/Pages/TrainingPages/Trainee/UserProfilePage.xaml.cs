using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class UserProfilePage : ContentPage
{
	public UserProfilePage()
	{
		InitializeComponent();
		BindingContext = new UserProfileViewModel();
	}
}