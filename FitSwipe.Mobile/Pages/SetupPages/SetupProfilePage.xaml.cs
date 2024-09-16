namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class SetupProfilePage : ContentPage
{
	private string _mainColor1 = "LimeGreen";
	private string _mainColor2 = "Green";
    public string MainColor1 {
		get => _mainColor1;
		set
		{
			_mainColor1 = value;
			OnPropertyChanged(nameof(MainColor1));
		}	
	} 
	public string MainColor2 { 
		get => _mainColor2; 
		set
		{
			_mainColor2 = value;
			OnPropertyChanged(nameof(MainColor2));
		}
	} 
    public SetupProfilePage()
	{
		InitializeComponent();
	}
}