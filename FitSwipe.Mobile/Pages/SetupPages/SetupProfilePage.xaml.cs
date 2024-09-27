using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class SetupProfilePage : ContentPage
{
	private string _mainColor1 = "LimeGreen";
	private string _mainColor2 = "Green";
	private string _mainColor3 = "#eef5e4";

    public UpdateUserProfileDto UserProfile { get; set; } = new UpdateUserProfileDto();

    public ObservableCollection<string> Jobs { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> Cities { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> Districts { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> Wards { get; set; } = new ObservableCollection<string>();

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
			OnPropertyChanged(nameof(MainColor3));
		}
	}
    public string MainColor3
    {
        get => _mainColor3;
        set
        {
            _mainColor3 = value;
            OnPropertyChanged(nameof(MainColor3));
        }
    }
    public SetupProfilePage()
	{
		InitializeComponent();
        // Populate Jobs and Cities lists with data
        Jobs = new ObservableCollection<string> { "Sinh viên", "Kinh doanh", "Công nghệ thông tin", "Khác" };
        Cities = new ObservableCollection<string> { "TPHCM", "Hà Nội", "Đà Nẵng" };
        Districts = new ObservableCollection<string> { "Quận 1", "Quận 2", "Quận 3" };
        Wards = new ObservableCollection<string> { "Phường 1", "Phường 2", "Phưởng 3" };
        JobPicker.ItemsSource = Jobs;
        CityPicker.ItemsSource = Cities;
        WardPicker.ItemsSource = Wards;
        DistrictPicker.ItemsSource = Districts;
        pageContent.IsVisible = false;
        CheckUser();
        BindingContext = UserProfile;
        // Initialize with default values
    }
    private async void CheckUser()
    {
        var token = await SecureStorage.GetAsync("auth_token");
        loadingDialog.IsVisible = true;
        if (token == null)
        {
            await DisplayAlert("Something wrong", "ERROR : User is not authenticated", "Ok");
            await Shell.Current.GoToAsync("//SignIn");
        } else
        {
            try
            {
                var user = await Fetcher.GetAsync<GetUserDto>("api/authentication/who-am-i", token);
                if (user == null)
                {
                    await DisplayAlert("Lỗi", "Đã xảy ra lỗi, vui lòng đăng nhập lại", "OK");
                    await Shell.Current.GoToAsync("//SignIn");
                } else
                {
                    pageContent.IsVisible = true;
                    UserProfile.Role = user.Role;
                    if (UserProfile.Role == Role.PT)
                    {
                        MainColor1 = "#2E3192";
                        MainColor2 = "#1f00b8";
                        MainColor3 = "#e8eeff";
                    }
                }
               
                //await DisplayAlert("INFO" ,"User fetched :" + user, "Oki");
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR","Error when loading data : " + ex.Message, "OK");
            }
        }
        loadingDialog.IsVisible = false;
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        //DisplayAlert("Review",$"Your current info : Bio : {UserProfile.Bio},\n Job : {UserProfile.Job},\n DateOfBirh : {UserProfile.DateOfBirth},\n " +
        //    $"City : {UserProfile.City},\n District : {UserProfile.District},\n Ward : {UserProfile.Ward},\n Gender : {UserProfile.SelectedGender}","Ok baby");
        Navigation.PushModalAsync(new SetupProfileStep2Page(UserProfile));
    }

    private void MaleCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var radioButton = (RadioButton)sender;
        int radioValue = int.Parse((string)radioButton.Value);
        UserProfile.SelectedGender = (Gender)radioValue;

    }
}