using CommunityToolkit.Mvvm.ComponentModel;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class CustomersRequestedScheduleViewModel : ObservableObject
  {
    [ObservableProperty]
    private GetUserDto _user = new();

    [ObservableProperty]
    private GetUserRequestedScheduleDtos _trainingDetails = new();

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

      _trainingDetails = new GetUserRequestedScheduleDtos
      {
        StartDate = new DateTime(2024, 1, 10),
        EndDate = new DateTime(2024, 4, 10),
        TotalSessions = 10,
        TotalDuration = 30
      };
    }
  }
}
