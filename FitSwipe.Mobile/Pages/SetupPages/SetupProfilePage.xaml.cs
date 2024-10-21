using CommunityToolkit.Maui.Extensions;
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
    //public ObservableCollection<string> Cities { get; set; } = new ObservableCollection<string>();
    //public ObservableCollection<string> Districts { get; set; } = new ObservableCollection<string>();
    //public ObservableCollection<string> Wards { get; set; } = new ObservableCollection<string>();

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
        Jobs = new ObservableCollection<string> { "Học sinh, Sinh viên", "Lao động chân tay", "Sư phạm", "Y học", "Kĩ sư", "Kinh doanh", "Công nghệ thông tin", "Làm thuê làm mướn", "Làm việc văn phòng", "Hiện tại thất nghiệp", "Khác", "Không muốn chia sẻ" };
        //Cities = new ObservableCollection<string> { "TPHCM", "Hà Nội", "Đà Nẵng","Khác" };
        //Districts = new ObservableCollection<string> { "Quận 1", "Quận 2", "Quận 3" };
        //Wards = new ObservableCollection<string> { "Phường 1", "Phường 2", "Phưởng 3" };
        JobPicker.ItemsSource = Jobs;
        //CityPicker.ItemsSource = Cities;
        //WardPicker.ItemsSource = Wards;
        //DistrictPicker.ItemsSource = Districts;
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
                    if (user.City == null || user.City == string.Empty)
                    {
                        UserProfile.City = "Nhấn nút refresh để lấy vị trí";
                    }
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
    public async Task<bool> Validate()
    {
        if (UserProfile.SelectedGender == null)
        {
            await DisplayAlert("Thiếu thông tin", "Hãy vui lòng cung cấp giới tính của bạn", "OK");
            return false;
        }
        if (UserProfile.Job == null)
        {
            await DisplayAlert("Thiếu thông tin", "Hãy vui lòng cung cấp ít nhất 1 giá trị cho phần Nghề Nghiệp", "OK");
            return false;
        }
        else if (UserProfile.City == null || UserProfile.City == "Nhấn nút refresh để lấy vị trí")
        {
            await DisplayAlert("Thiếu thông tin", "Vui lòng vị trí để chúng tôi có thể đề xuất cho bạn những kết quả phù hợp", "OK");
            return false;
        } 
        else if ((DateTime.Now.Year - DateOfBirthPicker.Date.Year) <= 5)
        {
            await DisplayAlert("Thông tin sai lệch", "Người dùng cần ít nhất trên 5 tuổi", "OK");
            return false;
        }
        return true;
    }
    private async void Button_Clicked(object sender, EventArgs e)
    { 
        if (await Validate())
        {
            await Navigation.PushModalAsync(new SetupProfileStep2Page(UserProfile));
        }
    }

    private void MaleCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var radioButton = (RadioButton)sender;
        int radioValue = int.Parse((string)radioButton.Value);
        UserProfile.SelectedGender = (Gender)radioValue;

    }

    private async void btnGPS_Clicked(object sender, EventArgs e)
    {
        loadingDialog.IsVisible = true;
        try
        {
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            var location = await Geolocation.Default.GetLocationAsync(request, new CancellationTokenSource().Token);
            if (location != null)
            {
            
                    UserProfile.Longitude = location.Longitude;
                    UserProfile.Latitude = location.Latitude;
                    IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);

                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null)
                    {
                        UserProfile.City = placemark.SubAdminArea + ", " + placemark.AdminArea;
                    }
                }
            
            }
        catch

        {
            await DisplayAlert("Lỗi", "Vui lòng bật vị trí", "OK");
        }
        loadingDialog.IsVisible = false;
    }

    private void btnCamera_Clicked(object sender, EventArgs e)
    {
        userUploadAvatarModal.Setup(UserProfile);
        userUploadAvatarModal.Theme = MainColor1;
        userUploadAvatarModal.Show();
    }
}