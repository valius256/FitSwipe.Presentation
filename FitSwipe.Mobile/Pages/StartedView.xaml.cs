namespace FitSwipe.Mobile.Pages;

public partial class StartedView : ContentPage
{
	public StartedView()
	{
		InitializeComponent();
	}

    private async void btnPT_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//TrailerVideo");
    }

    private async void btnTrainee_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//TrailerVideo");
    }
}