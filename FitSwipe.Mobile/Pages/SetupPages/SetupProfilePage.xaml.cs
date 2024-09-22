using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class SetupProfilePage : ContentPage
{
	private string _mainColor1 = "LimeGreen";
	private string _mainColor2 = "Green";

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
			OnPropertyChanged(nameof(MainColor2));
		}
	} 
    public SetupProfilePage()
	{
		InitializeComponent();
        // Populate Jobs and Cities lists with data
        Jobs = new ObservableCollection<string> { "Sinh viên", "Kinh doanh", "Công nghệ thông tin", "Khác" };
        Cities = new ObservableCollection<string> { "TPHCM", "Hà Nội", "Đà Nẵng" };
        Districts = new ObservableCollection<string> { "Quận 1, Quận 2, Quận 3" };
        Wards = new ObservableCollection<string> { "Phường 1", "Phường 2", "Phưởng 3" };
        JobPicker.ItemsSource = Jobs;
        CityPicker.ItemsSource = Cities;
        WardPicker.ItemsSource = Wards;
        DistrictPicker.ItemsSource = Districts;

        BindingContext = UserProfile;
        // Initialize with default values
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