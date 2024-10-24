using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Mobile.Extensions;
using FitSwipe.Mobile.Pages.FeedbackPages;
using FitSwipe.Mobile.Pages.TrainingPages;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using Mapster;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class TraineeSchedulePage : ContentPage
{
    private GetUserDto? LoginedUser;
    private string Token = string.Empty;
    private bool _isInTraining = false;
    public bool IsInTraining
    {
        get => _isInTraining;
        set
        {
            _isInTraining = value;
            OnPropertyChanged(nameof(IsInTraining));
        }
    }
    private ObservableCollection<GetSlotDto> _slots = [];
    public ObservableCollection<GetSlotDto> Slots
    {
        get => _slots;
        set
        {
            _slots = value;
            OnPropertyChanged(nameof(Slots));
        }
    }
    private GetTrainingDetailDto? _currentTrainingDetail { get; set; }
    public GetTrainingDetailDto? CurrentTrainingDetail {
        get => _currentTrainingDetail;
        set
        {
            _currentTrainingDetail = value;
            OnPropertyChanged(nameof(CurrentTrainingDetail));
        }
    }

    public TraineeSchedulePage()
	{
		InitializeComponent();
        //Setup();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var currentToken = await SecureStorage.GetAsync("auth_token") ?? string.Empty;
        if (Helper.CheckTokenChanged(Token, currentToken))
        {
            Setup();
            return;
        }
    }
    private async void Setup()
    {
        pageContent.IsVisible = false;
        var token = await SecureStorage.GetAsync("auth_token");
        if (token != null)
        {
            Token = token;
            LoginedUser = await Shortcut.GetLoginedUser(token);
        }
        //GET CURRENT TRAINING
        try
        {
            var trainingResult = await Fetcher.GetAsync<GetTrainingDetailDto>("api/trainings/current-training", Token);
            CurrentTrainingDetail = trainingResult;
        }
        catch
        {
            CurrentTrainingDetail = null;
        }
        
        if (CurrentTrainingDetail != null)
        {
            _isInTraining = true;
        }
        await FetchData(true);
        BindingContext = this;
        pageContent.IsVisible = true;
    }
    private async Task FetchData(bool isInitial)
    {
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Đang lấy dữ liệu...";
        try
        {
            if (LoginedUser == null)
            {
                throw new Exception("Người dùng chưa đăng nhập!");
            }
            var start = isInitial ? Helper.GetFirstDayOfWeek() : timeTable.CurrentWeek.StartDate.ToDateTime(TimeOnly.MinValue);
            var end = isInitial ? Helper.GetLastDayOfWeek() : timeTable.CurrentWeek.EndDate.ToDateTime(TimeOnly.MaxValue);
            string url = $"api/Slot?Filter.CreateById={LoginedUser.FireBaseId}&Filter.StartTime={start.ToString("yyyy-MM-ddTHH:mm:ssZ")}&Filter.EndTime={end.ToString("yyyy-MM-ddTHH:mm:ssZ")}" +
                $"&Filter.Status=2&Filter.Status=3&Filter.Status=4&limit=500";
            var result = await Fetcher.GetAsync<PagedResult<GetSlotDto>>(url);
            if (result != null)
            {
                Slots = result.Items.ToObservableCollection();
                ColorSlots();
                timeTable.SetSlots(Slots);
            }
            
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
        }
        loadingDialog.IsVisible = false;
        timeTable.SetSlots(Slots);

    }
    public void ColorSlots()
    {
        foreach (var slot in Slots)
        {
            if (slot.Status == SlotStatus.Disabled)
            {
                slot.Color = "#000000";
            }
            else if (slot.Status == SlotStatus.OnGoing)
            {
                slot.Color = "#e3a702";
            }
            else if (slot.Status == SlotStatus.NotStarted)
            {
                slot.Color = "#55bb00";
            }
            else if (slot.Status == SlotStatus.Finished)
            {
                slot.Color = "#628a77";
            }
        }
    }
    private void btnClose_Clicked(object sender, EventArgs e)
    {

    }

    private void timeTable_SlotAction(object sender, SlotEventArgs e)
    {
        var border = e.Border;
        var slot = e.Slot;

        border.GestureRecognizers.Clear();
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (sender, e) =>
        {
            await Navigation.PushModalAsync(new SlotDetailPage(slot.Id));
        };
        border.GestureRecognizers.Add(tapGesture);
    }

    private async void timeTable_WeekChanged(object sender, EventArgs e)
    {
        await FetchData(false);
    }

    private async void btnFinish_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Chủ động kết thúc khóa huấn luyện", "Bạn có chắc chắn về hành động này chứ ?", "Có", "Không");
        if (answer)
        {
            loadingDialog.IsVisible = true;
            loadingDialog.Message = "Đang thực hiện...";
            try
            {
                await Navigation.PushModalAsync(new FeeedbackPage(_currentTrainingDetail.Adapt<GetTrainingWithTraineeAndPTDto>()));
                await Fetcher.PutAsync($"api/trainings/finishing", new GetTrainingDto(), Token);
                await DisplayAlert("Thành công", "Đã chủ động kết thúc khóa huấn luyện", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", "Có xảy ra lỗi. Err" + ex.Message, "OK");
            }
            loadingDialog.IsVisible = false;
        }
    }
}