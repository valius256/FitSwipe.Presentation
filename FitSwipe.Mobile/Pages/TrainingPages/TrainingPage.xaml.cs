using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class TrainingPage : ContentPage
{
	public TrainingPage()
	{
		InitializeComponent();
		BindingContext = new TrainingPageViewModel();
	}

    private void ImageButton_Pressed (object sender, EventArgs e)
    {

        if (sender is ImageButton button)
        {
            button.ScaleTo(1.12, 100); // Scale up when pressed
        }
    }

    private void ImageButton_Released (object sender, EventArgs e)
    {

        if (sender is ImageButton button)
        {
            button.ScaleTo(1, 100); // Scale back to normal
        }
    }
}