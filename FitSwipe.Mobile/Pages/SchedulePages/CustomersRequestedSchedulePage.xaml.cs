using FitSwipe.Mobile.Controls;
using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Dtos.Slots;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class CustomersRequestedSchedulePage : ContentPage
{
  public ObservableCollection<GetSlotDto> Slots { get; set; } = new ObservableCollection<GetSlotDto>();
    public CustomersRequestedScheduleViewModel ViewModel;
  public CustomersRequestedSchedulePage ()
  {
    InitializeComponent();
    ViewModel = new CustomersRequestedScheduleViewModel();
    FetchSlot();
    BindingContext = ViewModel;
  }

  public void FetchSlot ()
  {
    Slots = new ObservableCollection<GetSlotDto>()
    {
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,20,7,0,0), EndTime = new DateTime(2024,9,20,8,30,0), Color="#2E3291" },
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,19,8,0,0), EndTime = new DateTime(2024,9,19,9,30,0), Color="#2E3291" },
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,18,10,15,0), EndTime = new DateTime(2024,9,18,12,45,0), Color="#2E3291" },
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,16,7,0,0), EndTime = new DateTime(2024,9,16,8,30,0), Color="#2E3291" },
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,17,8,0,0), EndTime = new DateTime(2024,9,17,9,30,0), Color="#2E3291" },
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,15,6,45,0), EndTime = new DateTime(2024,9,15,8,15,0), Color="#2E3291" },
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,21,14,0,0), EndTime = new DateTime(2024,9,21,15,30,0), Color="#2E3291" },
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,22,9,0,0), EndTime = new DateTime(2024,9,22,10,30,0), Color="#2E3291" },
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,23,11,0,0), EndTime = new DateTime(2024,9,23,12,30,0), Color="#2E3291" },
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,24,16,30,0), EndTime = new DateTime(2024,9,24,18,0,0), Color="#2E3291" },
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,25,7,0,0), EndTime = new DateTime(2024,9,25,8,30,0), Color="#2E3291" },
        new GetSlotDto { Id = Guid.NewGuid(), StartTime = new DateTime(2024,9,26,13,45,0), EndTime = new DateTime(2024,9,26,15,15,0), Color="#2E3291" },
    };
    ViewModel.SetSlots(Slots);
    customers_timeTable.SetSlots(Slots);
  }
}
