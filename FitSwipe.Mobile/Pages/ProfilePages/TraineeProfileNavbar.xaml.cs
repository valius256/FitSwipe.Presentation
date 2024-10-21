namespace FitSwipe.Mobile.Pages.ProfilePages;

public partial class TraineeProfileNavbar : ContentView
{
    public static readonly BindableProperty ActiveTabProperty =
            BindableProperty.Create(nameof(ActiveTab), typeof(int), typeof(TraineeProfileNavbar), 0, propertyChanged: OnTabChanged);

    private string _tab1Color = "#000000";	
	private string _tab2Color = "#000000";	
	private string _tab3Color = "#000000";
    public int ActiveTab
    {
        get => (int)GetValue(ActiveTabProperty);
        set => SetValue(ActiveTabProperty, value);
    }
    public string Tab1Color
	{
		get => _tab1Color;
		set
		{
			_tab1Color = value;
			OnPropertyChanged(nameof(Tab1Color));
		}
	}
    public string Tab2Color
    {
        get => _tab2Color;
        set
        {
            _tab2Color = value;
            OnPropertyChanged(nameof(Tab2Color));
        }
    }
    public string Tab3Color
    {
        get => _tab3Color;
        set
        {
            _tab3Color = value;
            OnPropertyChanged(nameof(Tab3Color));
        }
    }
    public TraineeProfileNavbar()
	{
		InitializeComponent();
        BindingContext = this;
        
	}
    private void UpdateBackground()
    {
        switch (ActiveTab)
        {
            case 1:
                Tab1Color = "#52BB00";
                Tab2Color = "#000000";
                Tab3Color = "#000000";
                break;
            case 2:
                Tab1Color = "#000000";
                Tab2Color = "#52BB00";
                Tab3Color = "#000000";
                break;
            case 3:
                Tab1Color = "#000000";
                Tab2Color = "#000000";
                Tab3Color = "#52BB00";
                break;
        }
    }
    private static void OnTabChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TraineeProfileNavbar)bindable;
        control.UpdateBackground();
    }
    private async void tab1_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//TraineeProfilePage");
    }

    private async void tab2_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//TraineePaymentPage");
    }

    private async void tab4_Tapped(object sender, TappedEventArgs e)
    {
        SecureStorage.Remove("auth_token");
        SecureStorage.Remove("loginedUserId");
        await Shell.Current.GoToAsync("//SignIn");
    }

    private async void tab3_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//WithdrawRequestPage?isTrainee=true");
    }
}