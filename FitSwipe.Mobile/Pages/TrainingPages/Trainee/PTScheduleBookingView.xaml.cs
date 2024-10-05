using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class PTScheduleBookingView : ContentPage
{
    public ObservableCollection<GetSlotDto> Slots { get; set; } = new ObservableCollection<GetSlotDto>();
    private GetUserDto _loginedUser = new GetUserDto();
    private GetUserDto _ptUser = new GetUserDto();
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
    private string _ptId { get; set; }
    public PTScheduleBookingView(string ptId)
	{
		InitializeComponent();
        _ptId = ptId;
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
        await FetchSlots(true);
        pageContent.IsVisible = true;
    }
    private async Task FetchPT()
    {
        try
        {
            //something here
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra khi truy vấn thông tin PT. Err : " + ex.Message, "OK");
        }
    }
    private async Task FetchSlots(bool isInitial)
    {
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Đang lấy dữ liệu...";
        try
        {
            var start = isInitial ? Helper.GetFirstDayOfWeek() : timeTable.CurrentWeek.StartDate.ToDateTime(TimeOnly.MinValue);
            var end = isInitial ? Helper.GetLastDayOfWeek() : timeTable.CurrentWeek.EndDate.ToDateTime(TimeOnly.MaxValue);
            string url = $"api/Slot?Filter.CreateById={_ptId}&Filter.StartTime={start.ToString("yyyy-MM-ddTHH:mm:ssZ")}&Filter.EndTime={end.ToString("yyyy-MM-ddTHH:mm:ssZ")}&limit=500";
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
    }

    public void ColorSlots()
    {
        foreach (var slot in Slots)
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
    }
    private void TestButton_Clicked(object sender, EventArgs e)
    {
        bookSlotModal.Show();
    }

    private void btnApprove_Clicked(object sender, EventArgs e)
    {

    }

    private void btnClose_Clicked(object sender, EventArgs e)
    {

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
            bookSlotModal.SetTime(slot.StartTime, slot.EndTime);           
        };      

        border.GestureRecognizers.Add(tapGesture);
    }

    private async void timeTable_WeekChanged(object sender, EventArgs e)
    {
        await FetchSlots(false);
    }
}