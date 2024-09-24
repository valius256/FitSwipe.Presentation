using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class TrainingPage : ContentPage
{
	public TrainingPage()
	{
		InitializeComponent();
		BindingContext = new TrainingPageViewModel();
	}
}