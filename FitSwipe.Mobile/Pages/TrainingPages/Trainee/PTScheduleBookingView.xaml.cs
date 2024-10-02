using FitSwipe.Shared.Dtos.Slots;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class PTScheduleBookingView : ContentPage
{
    public ObservableCollection<GetSlotDto> Slots { get; set; } = new ObservableCollection<GetSlotDto>();

    public PTScheduleBookingView()
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
            new GetSlotDto { StartTime =  new DateTime(2024,9,20,7,0,0), EndTime = new DateTime(2024,9,20,8,30,0), Color="#FF2E3192" },
            new GetSlotDto { StartTime =  new DateTime(2024,9,19,7,0,0), EndTime = new DateTime(2024,9,19,8,0,0), Color="#FF2E3192" },
            new GetSlotDto { StartTime =  new DateTime(2024,9,18,10,15,0), EndTime = new DateTime(2024,9,18,12,45,0), Color="#FF2E3192" },
            new GetSlotDto { StartTime =  new DateTime(2024,9,18,15,30,0), EndTime = new DateTime(2024,9,18,17,30,0), Color="#FF2E3192" },
            new GetSlotDto { StartTime =  new DateTime(2024,9,16,7,0,0), EndTime = new DateTime(2024,9,16,8,30,0), Color="#FF2E3192" }
        };
        timeTable.SetSlots(Slots);
        
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
}