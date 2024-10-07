using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class PTScheduleBookingView : ContentPage
{
    public ObservableCollection<GetSlotDto> Slots { get; set; } = new ObservableCollection<GetSlotDto>();
    public ObservableCollection<GetSlotDto> SlotsOfTrainee { get; set; } = new ObservableCollection<GetSlotDto>();
    private MyPTListPage _myPTListPage;
    private GetUserDto _loginedUser = new GetUserDto();
    private GetUserDto _ptUser = new GetUserDto();
    private GetTrainingWithTraineeAndPTDto _training = new GetTrainingWithTraineeAndPTDto();
    private string _durationString = string.Empty;
    private int _totalDuration;
    private int _totalSlot;
    public GetUserDto LoginedUser { 
        get => _loginedUser; 
        set
        {
            _loginedUser = value;
            OnPropertyChanged(nameof(LoginedUser));
        } 
    }
    public GetUserDto PTUser
    {
        get => _ptUser;
        set
        {
            _ptUser = value;
            OnPropertyChanged(nameof(PTUser));
        }
    }
    public string DurationString
    {
        get => _durationString;
        set
        {
            _durationString = value;
            OnPropertyChanged(nameof(DurationString));
        }
    }
    public int TotalDuration
    {
        get => _totalDuration;
        set
        {
            _totalDuration = value;
            OnPropertyChanged(nameof(TotalDuration));
        }
    }
    public int TotalSlot
    {
        get => _totalSlot;
        set
        {
            _totalSlot = value;
            OnPropertyChanged(nameof(TotalSlot));
        }
    }
    public PTScheduleBookingView(GetTrainingWithTraineeAndPTDto training, MyPTListPage myPTListPage)
    {
        InitializeComponent();

        _myPTListPage = myPTListPage;
        _training = training;

        Setup();
        pageContent.IsVisible = false;
        BindingContext = this;
    }

    private async void Setup()
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (token != null)
        {
            var user = await Shortcut.GetLoginedUser(token);
            if (user != null)
            {
                LoginedUser = user;
            }
        }
        PTUser = _training.PT;

        await FetchTraineeSlot();
        await FetchSlots(true);
        pageContent.IsVisible = true;
    }
    private void UpdateOverview()
    {
        double duration = 0;

        foreach (var slot in SlotsOfTrainee)
        {
            duration += (slot.EndTime - slot.StartTime).TotalHours;
        }
        TotalSlot = SlotsOfTrainee.Count;
        TotalDuration = (int) duration;
        
        if (SlotsOfTrainee.Count > 0)
        {
            var slotOrdered = SlotsOfTrainee.OrderBy(s => s.StartTime);
            DurationString = slotOrdered.Last().EndTime.ToString("dd/MM/yyyy") + " - " + slotOrdered.First().StartTime.ToString("dd/MM/yyyy")  ;
        }
    }
    private async Task FetchTraineeSlot()
    {
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Đang lấy dữ liệu buổi tập...";
        try
        {
            var result = await Fetcher.GetAsync<PagedResult<GetSlotDto>>($"api/Slot?Filter.TraineeId={LoginedUser.FireBaseId}&Filter.TrainingId={_training.Id}&limit=500");
            if (result != null)
            {
                SlotsOfTrainee = result.Items.ToObservableCollection();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra khi truy vấn thông tin PT. Err : " + ex.Message, "OK");
        }
        loadingDialog.IsVisible = false;
    }
    private async Task FetchSlots(bool isInitial)
    {
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Đang lấy dữ liệu lịch PT...";
        try
        {
            var start = isInitial ? Helper.GetFirstDayOfWeek() : timeTable.CurrentWeek.StartDate.ToDateTime(TimeOnly.MinValue);
            var end = isInitial ? Helper.GetLastDayOfWeek() : timeTable.CurrentWeek.EndDate.ToDateTime(TimeOnly.MaxValue);
            //Get slots of PT
            string url = $"api/Slot?Filter.CreateById={_training.PTId}&Filter.StartTime={start.ToString("yyyy-MM-ddTHH:mm:ssZ")}&Filter.EndTime={end.ToString("yyyy-MM-ddTHH:mm:ssZ")}&Filter.SlotStatuses=0&limit=500";
            var result = await Fetcher.GetAsync<PagedResult<GetSlotDto>>(url);
            if (result != null)
            {
                Slots = result.Items.ToObservableCollection();
                MergeAndUpdateTimeTable();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
        }
        loadingDialog.IsVisible = false;
    }
    private void btnApprove_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnClose_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void timeTable_SlotAction(object sender, Controls.SlotEventArgs e)
    {
        var border = e.Border;
        var slot = e.Slot;

        border.GestureRecognizers.Clear();
        var tapGesture = new TapGestureRecognizer();

        tapGesture.Tapped += (sender, e) =>
        {
            bookSlotModal.Show();
            if (slot.Status == SlotStatus.Unbooked)
            {
                bookSlotModal.SetTime(slot.StartTime, slot.EndTime, slot.Id, null);
            }
            else
            {
                var baseSlot = GetBaseSlot(slot);
                if (baseSlot != null)
                {
                    bookSlotModal.SetTime(baseSlot.StartTime, baseSlot.EndTime, baseSlot.Id, slot.Id,slot.StartTime, slot.EndTime);
                } else
                {
                    bookSlotModal.SetTime(DateTime.MinValue, DateTime.MaxValue,null, slot.Id, slot.StartTime, slot.EndTime);
                }
            }
        };      

        border.GestureRecognizers.Add(tapGesture);
    }

    private async void timeTable_WeekChanged(object sender, EventArgs e)
    {
        await FetchSlots(false);
    }
    private GetSlotDto? GetBaseSlot(GetSlotDto checkkingSlot)
    {
        foreach (var slot in Slots)
        {
            if (checkkingSlot.StartTime >= slot.StartTime && checkkingSlot.EndTime <= slot.EndTime)
            {
                return slot;
            }
        }
        return null;
    }
    private async void bookSlotModal_OnBookSlot(object sender, BookSlotModal.BookSlotEventArgs e)
    {
        var timeStart = e.TimeBegin;
        var timeEnd = e.TimeEnd;
        var startTime = new DateTime(e.Start.Year,e.Start.Month,e.Start.Day,timeStart.Hours,timeStart.Minutes,0,DateTimeKind.Utc);
        var endTime = new DateTime(e.End.Year, e.End.Month, e.End.Day, timeEnd.Hours, timeEnd.Minutes, 0,DateTimeKind.Utc);

        var answer = await DisplayAlert("Thêm khung giờ", "Bạn có chắc chắn về hành động này?", "Có", "Không");
        if (answer)
        {
            if (startTime < e.Start || endTime > e.End)
            {
                await DisplayAlert("Lỗi", "Vui lòng chọn thời gian diễn ra trong khung giờ", "OK");
            }
            else if (startTime <= DateTime.Now)
            {
                await DisplayAlert("Lỗi", "Vui lòng chọn khung thời gian chưa diễn ra", "OK");
            }
            else if (Helper.IsConflict(startTime, endTime, SlotsOfTrainee))
            {
                await DisplayAlert("Lỗi", "Đã bị trùng giờ. Vui lòng kiểm tra lại", "OK");
            } 
            else if (e.BaseSlotId == null)
            {
                await DisplayAlert("Lỗi", "Bạn đang đặt ngoài khung giờ của PT", "OK");
            }
            else
            {
                loadingDialog.IsVisible = true;
                loadingDialog.Message = "Đang thực hiện...";
                try
                {
                    var token = await SecureStorage.GetAsync("auth_token");
                    if (token != null)
                    {
                        var slots = await Fetcher.PostAsync<List<RequestCreateTrainingSlotDto>, List<GetSlotDto>>($"api/Slot/{_training.Id}/create", new List<RequestCreateTrainingSlotDto>
                        {
                            new RequestCreateTrainingSlotDto
                            {
                                StartTime = startTime,
                                EndTime = endTime,
                                BaseSlotId = e.BaseSlotId.Value
                            }
                        }, token);
                        if (slots != null)
                        {
                            SlotsOfTrainee.Add(slots[0]);
                            MergeAndUpdateTimeTable();
                            await DisplayAlert("Thành công", "Đã thêm thành công", "OK");
                            bookSlotModal.Hide();
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi", "Có lỗi đã xảy ra. Err : " + ex.Message, "OK");
                }
                loadingDialog.IsVisible = false;
            }
        }
    }

    private void MergeAndUpdateTimeTable()
    {
        var slots = Slots.ToObservableCollection();
        foreach (var slotDto in SlotsOfTrainee)
        {
            slots.Add(slotDto);
        }
        foreach (var slot in slots)
        {
            if (slot.Status == SlotStatus.Unbooked)
            {
                slot.Color = "#2E3192";
                if (slot.StartTime < DateTime.Now)
                {
                    slot.Color = "#636362";
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
        UpdateOverview();
        timeTable.SetSlots(slots);
    }

    private async void bookSlotModal_OnEditSlot(object sender, BookSlotModal.BookSlotEventArgs e)
    {
        var timeStart = e.TimeBegin;
        var timeEnd = e.TimeEnd;
        var startTime = new DateTime(e.Start.Year, e.Start.Month, e.Start.Day, timeStart.Hours, timeStart.Minutes, 0, DateTimeKind.Utc);
        var endTime = new DateTime(e.End.Year, e.End.Month, e.End.Day, timeEnd.Hours, timeEnd.Minutes, 0, DateTimeKind.Utc);
        var slotId = e.EditSlotId;

        var answer = await DisplayAlert("Chỉnh sửa khung giờ", "Bạn có chắc chắn về hành động này?", "Có", "Không");
        if (answer)
        {
            var currentSlot = SlotsOfTrainee.FirstOrDefault(s => s.Id == slotId);

            if (currentSlot == null)
            {
                await DisplayAlert("Lỗi", "Không tìm thấy khung giờ này", "OK");
            }
            else if (e.BaseSlotId != null && (startTime <= e.Start || endTime >= e.End))
            {
                await DisplayAlert("Lỗi", "Vui lòng chọn thời gian diễn ra trong khung giờ", "OK");
            }
            else if (startTime <= DateTime.Now)
            {
                await DisplayAlert("Lỗi", "Vui lòng chọn khung thời gian chưa diễn ra", "OK");
            }
            else if (Helper.IsConflict(startTime, endTime, SlotsOfTrainee, currentSlot))
            {
                await DisplayAlert("Lỗi", "Đã bị trùng giờ. Vui lòng kiểm tra lại", "OK");
            }
            else
            {
                loadingDialog.IsVisible = true;
                loadingDialog.Message = "Đang thực hiện...";
                try
                {
                    var token = await SecureStorage.GetAsync("auth_token");
                    if (token != null)
                    {
                        await Fetcher.PutAsync($"api/Slot/time", new RequestUpdateSlotTimeDto
                        {
                            SlotId = currentSlot.Id,
                            StartTime = startTime,
                            EndTime = endTime
                        }, token);
                        currentSlot.StartTime = startTime;
                        currentSlot.EndTime = endTime;
                        MergeAndUpdateTimeTable();
                        await DisplayAlert("Thành công", "Chỉnh sửa thành công", "OK");
                        bookSlotModal.Hide();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi", "Có lỗi đã xảy ra. Err : " + ex.Message, "OK");
                }
                loadingDialog.IsVisible = false;
            }
        }
    }

    private async void bookSlotModal_OnDeleteSlot(object sender, BookSlotModal.BookSlotEventArgs e)
    {
        var slotId = e.EditSlotId;

        var answer = await DisplayAlert("Xóa khung giờ", "Bạn có chắc chắn về hành động này?", "Có", "Không");
        if (answer)
        {
            var currentSlot = SlotsOfTrainee.FirstOrDefault(s => s.Id == slotId);
            if (currentSlot != null)
            {
                loadingDialog.IsVisible = true;
                loadingDialog.Message = "Đang thực hiện...";
                try
                {
                    var token = await SecureStorage.GetAsync("auth_token");
                    if (token != null)
                    {
                        await Fetcher.DeleteAsync($"api/Slot/{currentSlot.Id}", token);
                        SlotsOfTrainee.Remove(currentSlot);
                        MergeAndUpdateTimeTable();
                        await DisplayAlert("Thành công", "Đã xóa thành công", "OK");
                        bookSlotModal.Hide();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi", "Có lỗi đã xảy ra. Err : " + ex.Message, "OK");
                }
                loadingDialog.IsVisible = false;
            }
        }
    }
}