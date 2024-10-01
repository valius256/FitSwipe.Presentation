using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Dtos.Slots;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class PTSchedulePage : ContentPage
{
    public ObservableCollection<GetSlotDto> Slots { get; set; } = new ObservableCollection<GetSlotDto>();

    public PTSchedulePage()
    {
        InitializeComponent();
        SetupTimeTable();
        FetchSlots();
    }

    private void SetupTimeTable()
    {
        timeTable.Mode = 1;
    }

    private void FetchSlots()
    {
        Slots = new ObservableCollection<GetSlotDto>()
        {
            new GetSlotDto { StartTime =  new DateTime(2024,9,20,7,0,0), EndTime = new DateTime(2024,9,20,8,30,0), Color="#FF2E3191" },
            new GetSlotDto { StartTime =  new DateTime(2024,9,19,7,0,0), EndTime = new DateTime(2024,9,19,8,0,0), Color="#FF2E3191" },
            new GetSlotDto { StartTime =  new DateTime(2024,9,18,10,15,0), EndTime = new DateTime(2024,9,18,12,45,0), Color="#FF2E3191" },
            new GetSlotDto { StartTime =  new DateTime(2024,9,18,15,30,0), EndTime = new DateTime(2024,9,18,17,30,0), Color="#FF2E3191" },
            new GetSlotDto { StartTime =  new DateTime(2024,9,16,7,0,0), EndTime = new DateTime(2024,9,16,8,30,0), Color="#FF2E3191" }
        };
        timeTable.SetSlots(Slots);

    }

    private void btnClose_Clicked(object sender, EventArgs e)
    {

    }

    private async void timeTable_WeekChanged(object sender, EventArgs e)
    {
        var timeTable = sender as TimeTable;
        if (timeTable != null) 
        {
            await DisplayAlert("Selected Week",$"{timeTable.CurrentWeek.Display}","OKE");
        }
    }

    private void btnAdd_Tapped(object sender, TappedEventArgs e)
    {
        addSlotModal.Show();
    }

    private void btnDuplicate_Tapped(object sender, TappedEventArgs e)
    {
        duplicateSlotModal.Show();
    }

    private async void addSlotModal_OnAdded(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Thêm khung giờ", "Bạn có chắc chắn về hành động này?", "Có", "Không");
        if (answer)
        {
            var addModal = sender as PTAddSlotModal;
            if (addModal != null)
            {
                var timeFrames = addModal.GetTimeFrame();
                if (timeFrames[0] >= timeFrames[1])
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn thời gian bắt đầu nhỏ hơn thời gian kết thúc","OK");
                } else if (timeFrames[0].Hour < 4 && timeFrames[1].Hour > 22)
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn khung thời gian từ 4h - 22h", "OK");
                } else
                {
                    //Test add slot
                    Slots.Add(new GetSlotDto
                    {
                        StartTime = timeFrames[0],
                        EndTime = timeFrames[1],
                        Color = "FF2E3191"
                    });
                    timeTable.SetSlots(Slots);
                    timeTable.GotoWeek(new DateOnly(timeFrames[0].Year, timeFrames[0].Month, timeFrames[0].Day));
                    addModal.Hide();
                }
            }
        }

    }

    private void btnDelete_Tapped(object sender, TappedEventArgs e)
    {

    }
}