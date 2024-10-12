using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using FitSwipe.Mobile.Controls;
using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using Mapster;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class CustomersRequestedSchedulePage : ContentPage
{
    private string? _token;
    private string? _currentUserId;
    private Guid _trainingId;
    private Func<Task> _refresh;
    private ObservableCollection<GetSlotDto> _slots = [];
    private GetTrainingDetailDto _trainingDetails = new GetTrainingDetailDto();

    public GetTrainingDetailDto TrainingDetails
    {
        get => _trainingDetails;
        set {
            _trainingDetails = value;
            OnPropertyChanged(nameof(TrainingDetails));
        }
    }
      //public CustomersRequestedScheduleViewModel ViewModel;
    public CustomersRequestedSchedulePage(Guid trainingId, Func<Task> refresh)
    {
        InitializeComponent();
        _trainingId = trainingId;
        _refresh = refresh;
        Setup();
    }
    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void customers_timeTable_WeekChanged(object sender, EventArgs e)
    {
        await FetchSlot(false);

    }
    public async void Setup()
    {
        pageContent.IsVisible = false;
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Đang lấy dữ liệu...";
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token == null)
            {
                throw new Exception();
            }
            var user = (await Shortcut.GetLoginedUser(token));
            if (user != null)
            {
                _currentUserId = user.FireBaseId;
            }
            _token = token;
            await FetchTrainingDetail();
            await FetchSlot(true);
        }
        catch
        {
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Lỗi", "Lỗi xác thực. Vui lòng đăng nhập lại", "OK");
            }
            await Shell.Current.GoToAsync("//SignIn");
        }
        loadingDialog.IsVisible = false;
        pageContent.IsVisible = true;
        BindingContext = this;
    }
    public async Task FetchTrainingDetail()
    {
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Đang lấy thông tin...";
        try
        {
            if (_token != null)
            {
                var result = await Fetcher.GetAsync<GetTrainingDetailDto>($"api/trainings/{_trainingId}", _token);
                if (result != null)
                {
                    TrainingDetails = result;
                }
            }
        }
        catch (Exception ex)
        {
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
            }
        }
        loadingDialog.IsVisible = false;
    }
    public async Task FetchSlot(bool isInitial)
    {
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Vui lòng chờ...";
        try
        {
            if (_token != null && _currentUserId != null)
            {
                var start = isInitial ? Helper.GetFirstDayOfWeek() : customers_timeTable.CurrentWeek.StartDate.ToDateTime(TimeOnly.MinValue);
                var end = isInitial ? Helper.GetLastDayOfWeek() : customers_timeTable.CurrentWeek.EndDate.ToDateTime(TimeOnly.MaxValue);
                string url = $"api/Slot?Filter.CreateById={_currentUserId}&Filter.StartTime={start.ToString("yyyy-MM-ddTHH:mm:ssZ")}&Filter.EndTime={end.ToString("yyyy-MM-ddTHH:mm:ssZ")}&limit=500";
                var result = await Fetcher.GetAsync<PagedResult<GetSlotDto>>(url);
                if (result != null)
                {
                    var slots = result.Items.ToList();
                    slots.AddRange(TrainingDetails.Slots);
                    _slots = slots.ToObservableCollection();
                    FormatSlot();
                }
            }
        }
        catch (Exception ex)
        {
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
            }
        }
        loadingDialog.IsVisible = false;
    }

    private void FormatSlot()
    {
        foreach (var slot in _slots)
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
                if (slot.StartTime < DateTime.Now)
                {
                    slot.Color = "#cdcdcd";
                }
            }
        }
        customers_timeTable.SetSlots(_slots);
    }

    private async void btnYes_Clicked(object sender, EventArgs e)
    {
        if (TrainingDetails.Slots.Count > 0)
        {
            var starTime = TrainingDetails.Slots.OrderBy(x => x.StartTime).First().StartTime;
            starTime = DateOnly.FromDateTime(starTime).ToDateTime(TimeOnly.MinValue);
            var endTime = TrainingDetails.Slots.OrderBy(x => x.StartTime).Last().EndTime;
            endTime = DateOnly.FromDateTime(endTime).ToDateTime(TimeOnly.MaxValue);

            loadingDialog.IsVisible = true;
            loadingDialog.Message = "Vui lòng chờ...";
            try
            {
                if (_token != null && _currentUserId != null)
                {
                    string url = $"api/Slot?Filter.CreateById={_currentUserId}&Filter.StartTime={starTime.ToString("yyyy-MM-ddTHH:mm:ssZ")}&Filter.EndTime={endTime.ToString("yyyy-MM-ddTHH:mm:ssZ")}&limit=500";
                    var result = await Fetcher.GetAsync<PagedResult<GetSlotDto>>(url,_token);
                    if (result != null)
                    {
                        foreach (var slotTraining in TrainingDetails.Slots)
                        {
                            bool redFlag = true;
                            foreach (var slot in result.Items)
                            {
                                if (slotTraining.StartTime >= slot.StartTime &&  slotTraining.EndTime <= slot.EndTime)
                                {
                                    redFlag = false;
                                }
                            }
                            if (redFlag)
                            {
                                throw new Exception("Có ít nhất 1 khung giờ học viên yêu cầu nằm ngoài khung giờ rảnh của bạn. Vui lòng kiểm tra lại.");
                            }
                        }
                    }
                    await Navigation.PushModalAsync(new PTAcceptSchedule(_trainingId, _refresh));
                }
            }
            catch (Exception ex)
            {
                if (Application.Current != null && Application.Current.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                }
            }
            loadingDialog.IsVisible = false;
        }
    }
}
