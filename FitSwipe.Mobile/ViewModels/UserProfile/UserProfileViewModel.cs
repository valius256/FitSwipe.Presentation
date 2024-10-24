using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.Controls;
using FitSwipe.Mobile.Pages.ProfilePages;
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
using System.Windows.Input;

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
        [ObservableProperty]
        public bool isRefreshing = false;


        private RefreshView pageContent;
        private LoadingDialog loadingDialog;
        private TagPickerModal tagPickerModal;
        private ObservableCollection<GetTagDto> tags = [];
        public string? GuestId;
        public bool IsTagsChanged = false;

        public IRelayCommand RemoveTag { get; }
        public IRelayCommand AddTag { get; }
        public ICommand RefreshCommand { get; }

        // Constructor to initialize with sample data
        public UserProfileViewModel(RefreshView scrollView, LoadingDialog loadingDialog, TagPickerModal tagPickerModal, string? guestId = null)
        {
            RemoveTag = new RelayCommand<Guid>(RemoveATag);
            AddTag = new RelayCommand<string?>(AddFromTagType);
            RefreshCommand = new Command(OnRefresh);

            pageContent = scrollView;
            this.loadingDialog = loadingDialog;
            this.tagPickerModal = tagPickerModal;
            this.GuestId = guestId;
            GuestId = guestId;


            Jobs = new ObservableCollection<string> { "Học sinh, Sinh viên", "Lao động chân tay", "Sư phạm", "Y học", "Kĩ sư", "Kinh doanh", "Công nghệ thông tin", "Làm thuê làm mướn", "Làm việc văn phòng", "Hiện tại thất nghiệp", "Khác", "Không muốn chia sẻ" };
            // Handle property changed for CurrentImage to update SelectedImage
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(CurrentImage))
                {
                    SelectedImage = CurrentImage;
                }
            };
            //Setup();
        }
        public async void Setup()
        {
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                pageContent.IsVisible = false;
                loadingDialog.IsVisible = true;
                try
                {
                    if (GuestId == null)
                    {
                        var tagsResult = await Fetcher.GetAsync<List<GetTagDto>>("api/tags");
                        if (tagsResult != null)
                        {
                            tags = tagsResult.ToObservableCollection();
                        }
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
                    string? userId = null;
                    if (token != null && GuestId == null)
                    {
                        userId = await SecureStorage.GetAsync("loginedUserId");
                    }
                    else
                    {
                        userId = GuestId;
                    }
                    if (token == null || userId == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Lỗi", "Lỗi xác thực. Vui lòng đăng nhập lại!", "OK");
                        await Shell.Current.GoToAsync("//SignIn");
                        return;
                    }
                    //GEt user detail
                    var userDetail = await Fetcher.GetAsync<GetUserDetailDto>($"api/users/{userId}/detail");
                    if (userDetail != null)
                    {
                        User.FireBaseId = userDetail.FireBaseId;
                        if (User.UserName != userDetail.UserName) User.UserName = userDetail.UserName;
                        if (User.Email != userDetail.Email) User.Email = userDetail.Email;
                        if (User.Bio != userDetail.Email) User.Bio = userDetail.Bio;
                        if (User.Gender != userDetail.Gender) User.Gender = userDetail.Gender;
                        if (User.Job != userDetail.Job) User.Job = userDetail.Job;
                        if (User.AvatarUrl != userDetail.AvatarUrl) User.AvatarUrl = userDetail.AvatarUrl;
                        if (User.DateOfBirth != userDetail.DateOfBirth) User.DateOfBirth = userDetail.DateOfBirth;
                        if (User.City != userDetail.City) User.City = userDetail.City;

                        Updater = User.Adapt<RequestSetupProfileDto>();
                    }
                    //Get current training
                    if (isOwner)
                    {
                        GetTrainingDetailDto? currentTraining;
                        try
                        {
                            currentTraining = await Fetcher.GetAsync<GetTrainingDetailDto>($"api/trainings/current-training", token!);
                            CurrentTraining = currentTraining;
                            IsShowTrainingSection = true;
                            //Setup the training
                            ColorTrainingStatus();
                        }
                        catch
                        {
                            currentTraining = null;
                        }
                    }
                    if (userDetail != null)
                    {
                        loadingDialog.IsVisible = false;
                        pageContent.IsVisible = true;
                        AppendTags(userDetail.Tags);
                        AppendUserMedias(userDetail.UserMedias);
                        // Set the initial current image
                        CurrentImage = userDetail.UserMedias.FirstOrDefault();

                        // Set SelectedImage to the initial CurrentImage
                        SelectedImage = CurrentImage;
                    }
                }
                catch (Exception ex)
                {

                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");

                }
                loadingDialog.IsVisible = false;
            }
        }
        private void AppendTags(ObservableCollection<GetTagDto> tags)
        {
            var userTagList = User.Tags.ToList();
            foreach (var tag in userTagList)
            {
                var existedTag = tags.FirstOrDefault(t => t.Id == tag.Id);
                if (existedTag == null)
                {
                    User.Tags.Remove(tag);
                }
            }
            foreach (var tag in tags)
            {
                var existedTag = User.Tags.FirstOrDefault(t => t.Id == tag.Id);
                if (existedTag != null)
                {
                    if (existedTag.Name != tag.Name)
                    {
                        existedTag.Name = tag.Name;
                    }
                }
                else
                {
                    User.Tags.Add(tag);
                }
            }

            foreach (var tag in User.Tags)
            {
                tag.TagColor = GetRandomColor();
            }
        }
        private void AppendUserMedias(ObservableCollection<GetUserMediaDto> medias)
        {
            var mediaTagList = User.UserMedias.ToList();
            foreach (var media in mediaTagList)
            {
                var existedMedia = tags.FirstOrDefault(m => m.Id == media.Id);
                if (existedMedia == null)
                {
                    User.UserMedias.Remove(media);
                }
            }
            foreach (var media in medias)
            {
                var existedMedia = User.UserMedias.FirstOrDefault(m => m.Id == media.Id);
                if (existedMedia != null)
                {
                    if (existedMedia.Description != existedMedia.Description)
                    {
                        existedMedia.Description = existedMedia.Description;
                    }
                }
                else
                {
                    User.UserMedias.Add(media);
                }
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
        private async void OnRefresh()
        {
            if (!loadingDialog.IsVisible)
            {
                // Stop the refresh animation
                IsRefreshing = false;
                await FetchData();
            }
        }
        [RelayCommand]
        public async Task GetGPS()
        {
            loadingDialog.IsVisible = true;
            try
            {
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

            }
            catch

            {
                if (Application.Current != null && Application.Current.MainPage != null && updater.UserName != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Vui lòng bật vị trí", "OK");
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
