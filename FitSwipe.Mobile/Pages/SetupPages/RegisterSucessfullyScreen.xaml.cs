using Firebase.Auth;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;

namespace FitSwipe.Mobile.Pages.SetupPages;

[QueryProperty(nameof(PassedRole), "role")]
public partial class RegisterSucessfullyScreen : ContentPage
{
    public string PassedRole { get; set; } = string.Empty;

    private string _mainColor1 = "LimeGreen";
    private string _mainColor2 = "LightGreen";
    public RegisterSucessfullyScreen()
    {
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Use the UserId here
        if (PassedRole == "pt")
        {
            MainColor1 = "#2E3192";
            MainColor2 = "#1f00b8";
        }
    }
    public string MainColor1
    {
        get => _mainColor1;
        set
        {
            _mainColor1 = value;
            OnPropertyChanged(nameof(MainColor1));
        }
    }
    public string MainColor2
    {
        get => _mainColor2;
        set
        {
            _mainColor2 = value;
            OnPropertyChanged(nameof(MainColor2));
        }
    }

    private void btnReady_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//SignIn");
    }
}