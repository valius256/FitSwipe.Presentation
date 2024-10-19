﻿
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.Controls;
using FitSwipe.Mobile.Pages.ProfilePages;
using FitSwipe.Shared.Dtos.Medias;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using Mapster;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FitSwipe.Mobile.ViewModels.UserProfile
{
    public partial class PTProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<GetTagDto> userTags = new();
        [ObservableProperty]
        private ObservableCollection<GetTrainingFeedbackDetailDto> trainingFeedbacks = new();

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
        private List<GetTrainingDetailDto> currentTrainings = [];

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
        [ObservableProperty]
        private bool isFetchingComment = false;
        [ObservableProperty]
        public ObservableCollection<GetSlotDto> slots = [];

        private RefreshView pageContent;
        private LoadingDialog loadingDialog;
        private TagPickerModal tagPickerModal;
        private TimeTable timeTable;
        private ObservableCollection<GetTagDto> tags = [];
        public bool IsTagsChanged = false;
        private string? _guestId;
        public int CurrentRatingPage = 0;
        public int PageRatingSize = 5;
        public int TotalRatingPage = 1;

        public IRelayCommand RemoveTag { get; }
        public IRelayCommand AddTag { get; }
        public ICommand RefreshCommand { get; }

        // Constructor to initialize with sample data
        public PTProfileViewModel(RefreshView scrollView, LoadingDialog loadingDialog, TagPickerModal tagPickerModal, TimeTable timeTable, string? guestId = null)
        {
            RemoveTag = new RelayCommand<Guid>(RemoveATag);
            AddTag = new RelayCommand<string?>(AddFromTagType);
            RefreshCommand = new Command(OnRefresh);

            pageContent = scrollView;
            this.loadingDialog = loadingDialog;
            this.tagPickerModal = tagPickerModal;
            this.timeTable = timeTable;
            _guestId = guestId;

            Jobs = new ObservableCollection<string> { "Học sinh, Sinh viên", "Lao động chân tay", "Sư phạm", "Y học", "Kĩ sư", "Kinh doanh", "Công nghệ thông tin", "Làm thuê làm mướn", "Làm việc văn phòng", "Hiện tại thất nghiệp", "Khác", "Không muốn chia sẻ" };
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
                    var tagsResult = await Fetcher.GetAsync<List<GetTagDto>>("api/tags");
                    if (tagsResult != null)
                    {
                        tags = tagsResult.ToObservableCollection();
                    }
                    await FetchData();
                    pageContent.IsVisible = true;
                    await FetchSlots(true);               
                    await FetchRatingData(true);
                }
                catch (Exception ex)
                {

                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");

                }
                loadingDialog.IsVisible = false;
                pageContent.IsVisible = true;
            }
        }
        public async Task FetchSlots(bool isInitial)
        {
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                loadingDialog.IsVisible = true;
                loadingDialog.Message = "Đang lấy dữ liệu lịch PT...";
                try
                {
                    var start = isInitial ? Helper.GetFirstDayOfWeek() : timeTable.CurrentWeek.StartDate.ToDateTime(TimeOnly.MinValue);
                    var end = isInitial ? Helper.GetLastDayOfWeek() : timeTable.CurrentWeek.EndDate.ToDateTime(TimeOnly.MaxValue);
                    //Get slots of PT
                    string url = $"api/Slot?Filter.CreateById={user.FireBaseId}&Filter.StartTime={start.ToString("yyyy-MM-ddTHH:mm:ssZ")}&Filter.EndTime={end.ToString("yyyy-MM-ddTHH:mm:ssZ")}&Filter.SlotStatuses=0&limit=500";
                    var result = await Fetcher.GetAsync<PagedResult<GetSlotDto>>(url);
                    if (result != null)
                    {
                        Slots = result.Items.ToObservableCollection();
                        ColorSlot();
                        timeTable.SetSlots(Slots);
                        timeTable.IsVisible = true;
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                }
                loadingDialog.IsVisible = false;
            }
        }
        private void ColorSlot()
        {
            foreach (var slot in Slots)
            {
                if (slot.Status == SlotStatus.Unbooked)
                {
                    slot.Color = "#2E3192";
                    if (slot.StartTime < DateTime.Now)
                    {
                        slot.Color = "#ededed";
                    }
                }
                else if (slot.Status == SlotStatus.Disabled)
                {
                    slot.Color = "#000000";
                }
                else if (slot.Status == SlotStatus.OnGoing)
                {
                    slot.Color = "#e3a702";
                }
                else if (slot.Status == SlotStatus.NotStarted)
                {
                    slot.Color = "#52BB00";
                }
                else if (slot.Status == SlotStatus.Finished)
                {
                    slot.Color = "#0fab60";
                }
                else if (slot.Status == SlotStatus.Pending)
                {
                    slot.Color = "#666666";
                }
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
                    if (token != null && _guestId == null)
                    {
                        user = await Shortcut.GetLoginedUser(token);
                    }
                    else
                    {
                        user = new GetUserDto { FireBaseId = _guestId };
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
                    var result = await Fetcher.GetAsync<PagedResult<GetTrainingDetailDto>>($"api/trainings?Filter.TrainingStatuses=2&Filter.TrainingStatuses=3", token);
                    if (result != null)
                    {
                        CurrentTrainings = result.Items.ToList();
                        foreach(var item in CurrentTrainings)
                        {
                            IsShowTrainingSection = true;
                            //Setup the training
                            ColorTrainingStatus(item);
                        }
                            
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
        private void ColorTrainingStatus(GetTrainingDetailDto trainingDetailDto)
        {
            if (trainingDetailDto != null)
            {
                if (trainingDetailDto.Status == TrainingStatus.NotStarted)
                {
                    trainingDetailDto.StatusString = "Sắp bắt đầu";
                    trainingDetailDto.StatusColor = "#969696";
                }
                if (trainingDetailDto.Status == TrainingStatus.OnGoing)
                {
                    trainingDetailDto.StatusString = "Đang diễn ra";
                    trainingDetailDto.StatusColor = "#52BB00";
                }
                double duration = 0;
                foreach (var item in trainingDetailDto.Slots)
                {
                    duration += (item.EndTime - item.StartTime).TotalHours;
                }
                CurrentTrainingSlotDisplay = $"{trainingDetailDto.Slots.Count} buổi, {(int)duration} tiếng";
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
        private async Task AppendRatingList(IList<GetTrainingFeedbackDetailDto> list)
        {
            foreach (var feedback in list) 
            {
                trainingFeedbacks.Add(feedback);
                await Task.Delay(200);
            }
        }
        public async Task FetchRatingData(bool isInitial)
        {
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                isFetchingComment = true;
                try
                {
                    var token = await SecureStorage.GetAsync("auth_token");
                    if (token == null)
                    {
                        throw new Exception("Có sự cố xảy ra. Vui lòng đăng nhập lại");
                    }
                    CurrentRatingPage += 1;
                    var response = await Fetcher.GetAsync<PagedResult<GetTrainingFeedbackDetailDto>>($"api/trainings/PT-feedback/{user.FireBaseId}?page={CurrentRatingPage}&limit={PageRatingSize}", token);
                    if (response != null)
                    {
                        await AppendRatingList(response.Items);
                        if (isInitial)
                        {
                            TotalRatingPage = (int)Math.Ceiling((double)response.Total / response.Limit);
                        } 
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                }
                isFetchingComment = false;
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

        [RelayCommand]
        private async void Match()
        {
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                loadingDialog.IsVisible = true;
                loadingDialog.Message = "Đang thực hiện...";
                try
                {
                    var token = await SecureStorage.GetAsync("auth_token");
                    if (token == null)
                    {
                        throw new Exception("Có sự cố xảy ra. Vui lòng đăng nhập lại");
                    }
                    await Fetcher.PostAsync($"api/trainings", new CreateTrainingDto
                    {
                        PTId = user.FireBaseId,
                        Status = Shared.Enums.TrainingStatus.Matched
                    }, token);
                }
                catch
                {
                    //await DisplayAlert("Lỗi", "Có sự cố xảy ra. Err : " + ex.Message, "OK");
                }
                loadingDialog.IsVisible = false;
            }
            await Shell.Current.GoToAsync("//MyPTList?flag=true");
        }
    }
}
