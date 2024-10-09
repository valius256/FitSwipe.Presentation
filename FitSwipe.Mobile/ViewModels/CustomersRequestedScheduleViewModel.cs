using CommunityToolkit.Mvvm.ComponentModel;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class CustomersRequestedScheduleViewModel : ObservableObject
  {
    [ObservableProperty]
    private GetUserDto _user = new();

    [ObservableProperty]
    private GetUserRequestedScheduleDto _trainingDetails = new();

    public CustomersRequestedScheduleViewModel ()
    {
      _user = new GetUserDto
      {
        UserName = "Nguyễn Thanh Phong",
        Job = "Sinh viên",
        AvatarUrl = "pt3.png",
        DateOfBirth = new DateTime(2003, 9, 21),
        City = "Hồ Chí Minh",
        District = "Quận 1",
        Ward = "Phường Bến Thành",
        Street = "Lê Lai",
        Role = Shared.Enums.Role.Trainee,
        Status = Shared.Enums.UserStatus.Active,
        CreatedDate = new DateTime(2021, 10, 10),
        RecordStatus = Shared.Enums.RecordStatus.Active
      };
    }

    public void SetSlots (ObservableCollection<GetSlotDto> getSlotDtos)
    {
      _trainingDetails = new GetUserRequestedScheduleDto
      {
        StartDate = getSlotDtos.First().StartTime,
        EndDate = getSlotDtos.Last().EndTime,
        TotalSessions = getSlotDtos.Count,
      };
      double totalDuration = 0;
      foreach (var slot in getSlotDtos)
      {
        totalDuration += (slot.EndTime - slot.StartTime).TotalHours;
      }
      _trainingDetails.TotalDuration = (int)totalDuration;
    }
  }
}
