using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Utils;
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
    }

    private void FetchSlots()
    {
        Slots = new ObservableCollection<GetSlotDto>()
        {
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,20,7,0,0), EndTime = new DateTime(2024,9,20,8,30,0), Color="#FF2E3191" },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,19,7,0,0), EndTime = new DateTime(2024,9,19,8,0,0), Color="#FF2E3191" },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,18,10,15,0), EndTime = new DateTime(2024,9,18,12,45,0), Color="#FF2E3191" },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,18,15,30,0), EndTime = new DateTime(2024,9,18,17,30,0), Color="#FF2E3191" },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,16,7,0,0), EndTime = new DateTime(2024,9,16,8,30,0), Color="#FF2E3191" }
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
                    await DisplayAlert("Lỗi", "Vui lòng chọn thời gian bắt đầu nhỏ hơn thời gian kết thúc", "OK");
                } 
                else if (timeFrames[0].Hour < 4 || timeFrames[1].Hour > 22)
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn khung thời gian từ 4h - 22h", "OK");
                } 
                else if (timeFrames[0] <= DateTime.Now)
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn khung thời gian chưa diễn ra", "OK");
                }
                else if (Helper.IsConflict(timeFrames[0], timeFrames[1], Slots))
                {
                    await DisplayAlert("Lỗi", "Đã bị trùng giờ. Vui lòng kiểm tra lại", "OK");
                }
                else
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

    private async void btnDelete_Tapped(object sender, TappedEventArgs e)
    {
        var answer = await DisplayAlert("Xóa hết khung giờ của tuần này", "Bạn có chắc chắn về hành động này?", "Có", "Không");
        if (answer)
        {
            //Test delete slot
            Slots.Clear();
            timeTable.SetSlots(Slots);
        }
    }

    private async void editSlotModal_OnAdded(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Chỉnh khung giờ", "Bạn có chắc chắn về hành động này?", "Có", "Không");
        if (answer)
        {
            var editModal = sender as PTAddSlotModal;
            if (editModal != null)
            {
                var timeFrames = editModal.GetTimeFrame();
                if (timeFrames[0] >= timeFrames[1])
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn thời gian bắt đầu nhỏ hơn thời gian kết thúc", "OK");
                }
                else if (timeFrames[0].Hour < 4 || timeFrames[1].Hour > 22)
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn khung thời gian từ 4h - 22h", "OK");
                }
                else if (timeFrames[0] <= DateTime.Now)
                {
                    await DisplayAlert("Lỗi", "Vui lòng chọn khung thời gian chưa diễn ra", "OK");
                }
                else if (Helper.IsConflict(timeFrames[0], timeFrames[1], Slots))
                {
                    await DisplayAlert("Lỗi", "Đã bị trùng giờ. Vui lòng kiểm tra lại", "OK");
                }
                else
                {
                    //Test edit slot
                    var slot = Slots.FirstOrDefault(s => editModal.RefSlot != null && s.Id ==  editModal.RefSlot.Id);
                    if (slot != null)
                    {
                        slot.StartTime = timeFrames[0];
                        slot.EndTime = timeFrames[1];
                        timeTable.SetSlots(Slots);
                        timeTable.GotoWeek(new DateOnly(timeFrames[0].Year, timeFrames[0].Month, timeFrames[0].Day));
                    }
                    editModal.Hide();
                }
            }
        }
    }

    private void timeTable_SlotAction(object sender, SlotEventArgs e)
    {
        var border = e.Border;
        var slot = e.Slot;

        border.GestureRecognizers.Clear();
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (sender, e) =>
        {
            editSlotModal.RefSlot = slot;
            editSlotModal.Show();
        };
        border.GestureRecognizers.Add(tapGesture);
    }

    private async void editSlotModal_OnDeleted(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Xóa khung giờ", "Bạn có chắc chắn về hành động này?", "Có", "Không");
        if (answer)
        {
            var editModal = sender as PTAddSlotModal;
            if (editModal != null)
            {
                //Test delete slot
                var slot = Slots.FirstOrDefault(s => editModal.RefSlot != null && s.Id == editModal.RefSlot.Id);
                if (slot != null)
                {
                    Slots.Remove(slot);
                    timeTable.SetSlots(Slots);
                }
                editModal.Hide();
            }
        }
    }
}