using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.Controls;
using FitSwipe.Mobile.Pages.ProfilePages;
using FitSwipe.Mobile.ViewModels.UserProfile;
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Medias;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using Mapster;
using Microsoft.Maui.Devices.Sensors;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class UserProfileViewModel : ObservableObject
  {
    [ObservableProperty]
    private ObservableCollection<GetTagDto> userTags = new();

    [ObservableProperty]
    private GetUserDetailDto user = new();

    [ObservableProperty]
    private RequestSetupProfileDto updater = new();
    //public UserProfileCombinedViewModel CombinedViewModel { get; set; }

    [ObservableProperty]
    private GetUserMediaDto? selectedImage;

    // Property to track the currently selected image in the carousel
    [ObservableProperty]
    private GetUserMediaDto? currentImage;

    [ObservableProperty]
    private GetTrainingDetailDto? currentTraining;

    [ObservableProperty]
    private string? currentTrainingSlotDisplay;

    [ObservableProperty]
    private bool isOwner = true;
    [ObservableProperty]
    private bool isShowTrainingSection = false;
    [ObservableProperty]
    private bool isEditName = false;
    [ObservableProperty]
    private bool isEditPersonalInformation = false;
    [ObservableProperty]
    private bool isEditBio = false;
    [ObservableProperty]
    private ObservableCollection<string> jobs = new();

    private ScrollView pageContent;
    private LoadingDialog loadingDialog;
    private TagPickerModal tagPickerModal;
    private ObservableCollection<GetTagDto> tags = [];
    public bool IsTagsChanged = false;

    public IRelayCommand RemoveTag { get; }
    public IRelayCommand AddTag { get; }

    // Constructor to initialize with sample data
    public UserProfileViewModel (ScrollView scrollView, LoadingDialog loadingDialog, TagPickerModal tagPickerModal)
    {
        RemoveTag = new RelayCommand<Guid>(RemoveATag);
        AddTag = new RelayCommand<string?>(AddFromTagType);

        pageContent = scrollView;
        this.loadingDialog = loadingDialog;
        this.tagPickerModal = tagPickerModal;

        Jobs = new ObservableCollection<string> { "Học sinh, Sinh viên", "Lao động chân tay", "Sư phạm" , "Y học" , "Kĩ sư" , "Kinh doanh", "Công nghệ thông tin",  "Làm thuê làm mướn" ,"Làm việc văn phòng","Hiện tại thất nghiệp", "Khác","Không muốn chia sẻ" };
        // Handle property changed for CurrentImage to update SelectedImage
        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(CurrentImage))
            {
                SelectedImage = CurrentImage;
            }
        };
        Setup();
    }
    private async void Setup()
    {
        if (Application.Current != null && Application.Current.MainPage != null)
        {
            pageContent.IsVisible = false;
            loadingDialog.IsVisible = true;
            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                GetUserDto? user = null;
                if (token != null)
                {
                    user = await Shortcut.GetLoginedUser(token);
                }
                if (token == null || user == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Lỗi xác thực. Vui lòng đăng nhập lại!", "OK");
                    await Shell.Current.GoToAsync("//SignIn");
                    return;
                }
                var tagsResult = await Fetcher.GetAsync<List<GetTagDto>>("api/tags");
                if (tagsResult != null)
                {
                    tags = tagsResult.ToObservableCollection();
                }
                await FetchData();
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");

            }
            loadingDialog.IsVisible = false;
            pageContent.IsVisible = true;
        }
    }
    public async Task FetchData()
    {
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                loadingDialog.IsVisible = true;
                try
                {
                    var token = await SecureStorage.GetAsync("auth_token");
                    GetUserDto? user = null;
                    if (token != null)
                    {
                        user = await Shortcut.GetLoginedUser(token);
                    }
                    if (token == null || user == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Lỗi", "Lỗi xác thực. Vui lòng đăng nhập lại!", "OK");
                        await Shell.Current.GoToAsync("//SignIn");
                        return;
                    }
                    //GEt user detail
                    var userDetail = await Fetcher.GetAsync<GetUserDetailDto>($"api/users/{user.FireBaseId}/detail");
                    if (userDetail != null)
                    {
                        User = userDetail;
                        Updater = User.Adapt<RequestSetupProfileDto>();
                        foreach (var tag in User.Tags)
                        {
                            tag.TagColor = GetRandomColor();
                        }
                    }
                    //Get current training
                    GetTrainingDetailDto? currentTraining;
                    try
                    {
                        currentTraining = await Fetcher.GetAsync<GetTrainingDetailDto>($"api/trainings/current-training", token);
                        CurrentTraining = currentTraining;
                        IsShowTrainingSection = true;
                        //Setup the training
                        ColorTrainingStatus();
                    } catch
                    {
                        currentTraining = null;
                    }
                    
                    // Set the initial current image
                    CurrentImage = User.UserMedias.FirstOrDefault();

                    // Set SelectedImage to the initial CurrentImage
                    SelectedImage = CurrentImage;
                }
                catch (Exception ex)
                {

                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");

                }
                loadingDialog.IsVisible = false;
            }
    } 
    private void ColorTrainingStatus()
    {
        if (CurrentTraining != null)
        {
            if (CurrentTraining.Status == TrainingStatus.NotStarted)
            {
                    CurrentTraining.StatusString = "Sắp bắt đầu";
                    CurrentTraining.StatusColor = "#969696";
            }
            if (CurrentTraining.Status == TrainingStatus.OnGoing)
            {
                    CurrentTraining.StatusString = "Đang diễn ra";
                    CurrentTraining.StatusColor = "#52BB00";
            }
            double duration = 0;
            foreach (var item in CurrentTraining.Slots)
            {
                duration += (item.EndTime - item.StartTime).TotalHours;
            }
            CurrentTrainingSlotDisplay = $"{CurrentTraining.Slots.Count} buổi, {(int)duration} tiếng";
        }      
    }

    private string GetRandomColor()
    {
        Random random = new Random();
        int red = random.Next(0, 256);
        int green = random.Next(0, 256);
        int blue = random.Next(0, 256);

        return $"#{red:X2}{green:X2}{blue:X2}";

    }
    public void UpsertTags(List<GetTagDto> tags)
    {
            foreach (var tag in tags)
            {
                if (!User.Tags.Contains(tag))
                {
                    tag.TagColor = GetRandomColor();
                    User.Tags.Add(tag);
                }
            }
            if (tags.Count > 0)
            {
                var tagType = tags[0].TagType;
                var tagOfTypes = User.Tags.Where(s => s.TagType == tags[0].TagType).ToList();
                foreach (var tag in tagOfTypes)
                {
                    if (!tags.Contains(tag))
                    {
                        User.Tags.Remove(tag);
                    }
                }
                
            }
            IsTagsChanged = true;
            
    }

    [RelayCommand]
    public async Task GetGPS()
    {
        loadingDialog.IsVisible = true;
        GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

        var location = await Geolocation.Default.GetLocationAsync(request, new CancellationTokenSource().Token);
        if (location != null)
        {
            Updater.Longitude = location.Longitude;
            Updater.Latitude = location.Latitude;
            IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);

            var placemark = placemarks?.FirstOrDefault();

            if (placemark != null)
            {
                Updater.City = placemark.SubAdminArea + ", " + placemark.AdminArea;
            }
        }
        loadingDialog.IsVisible = false;
    }
    [RelayCommand]
    public async Task UpdateProfile()
    {
            if (Application.Current != null && Application.Current.MainPage != null && updater.UserName != null)
            {
                loadingDialog.IsVisible = true;
                try
                {
                    var token = await SecureStorage.GetAsync("auth_token");
                    GetUserDto? user = null;
                    if (token != null)
                    {
                        user = await Shortcut.GetLoginedUser(token);
                    }
                    if (token == null || user == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Lỗi", "Lỗi xác thực. Vui lòng đăng nhập lại!", "OK");
                        await Shell.Current.GoToAsync("//SignIn");
                        return;
                    }
                    
                    await Fetcher.PatchAsync("api/users/set-up", updater, token);
                    User.UserName = updater.UserName;
                    User.Bio = updater.Bio;
                    User.DateOfBirth = updater.DateOfBirth;
                    User.Job = updater.Job;
                    User.City = updater.City;
                    User.Longitude = updater.Longitude;
                    User.Latitude = updater.Latitude;
                    if (IsTagsChanged)
                    {
                        var requestTags = new RequestUpsertTagsDto
                        {
                            NewTagIds = User.Tags.Select(t => t.Id).ToList()
                        };
                        await Fetcher.PutAsync("api/tags/upsert-user-tags", requestTags, token);
                        IsTagsChanged = false;
                    }
                    await Application.Current.MainPage.DisplayAlert("Thành công", "Đã cập nhật", "OK");
                }
                catch (Exception ex)
                {

                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");

                }
                loadingDialog.IsVisible = false;
            }
    }
    [RelayCommand]
    private void RemoveATag(Guid tagId)
    {
            var tag = User.Tags.FirstOrDefault(s => s.Id == tagId);
            if (tag != null)
            {
                User.Tags.Remove(tag);
                IsTagsChanged = true;
            }
    }
    [RelayCommand]
    private void AddFromTagType(string? tagType)
    {
            tagPickerModal.Tags = tags.Where(s => s.TagType.ToString() == tagType).ToObservableCollection();
            tagPickerModal.SetSelected(User.Tags.Where(s => s.TagType.ToString() == tagType).ToList());
            tagPickerModal.Show();
    }
  }
}
