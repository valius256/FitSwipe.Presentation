using FitSwipe.Shared.Dtos.Slots;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class PTAcceptSchedule : ContentPage
{
    public ObservableCollection<GetSlotDto> Slots { get; set; } = new ObservableCollection<GetSlotDto>();
    public PTAcceptSchedule()
	{
		InitializeComponent();
        GetSlots();
        BindingContext = this;
    }

	public void GetSlots()
	{
        Slots = new ObservableCollection<GetSlotDto>()
        {
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,20,7,0,0), EndTime = new DateTime(2024,9,20,8,30,0), Location = "Phòng tập City Gym" , TotalVideos = 5, Content="Push ups" },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,19,7,0,0), EndTime = new DateTime(2024,9,19,8,0,0), Location = "Phòng tập City Gym" , TotalVideos = 3, Content="Sit ups"  },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,18,10,15,0), EndTime = new DateTime(2024,9,18,12,45,0), Location = "Phòng tập City Gym" , TotalVideos = 5 , Content="Run 10km" },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,18,15,30,0), EndTime = new DateTime(2024,9,18,17,30,0), Location = "" , TotalVideos = 4, Content="Mix up"  },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,16,7,0,0), EndTime = new DateTime(2024,9,16,8,30,0) , Location = "" , TotalVideos = 5, Content="Pull ups"  }
        };
        for (var i = 0; i < Slots.Count; i++)
        {
            Slots[i].SlotNumber = i + 1;
            if (!string.IsNullOrEmpty(Slots[i].Location))
            {
                Slots[i].Color = "#FF2E3191";
            } else
            {
                Slots[i].Color = "Gray";
            }
        }
    }
}