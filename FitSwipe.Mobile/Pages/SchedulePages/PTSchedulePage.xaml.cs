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
}