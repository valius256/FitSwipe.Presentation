using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class TrainingPageViewModel : ObservableObject
  {
    [ObservableProperty]
    private ObservableCollection<GetTrainingDetailDto> _userList = new();


    private string _selectedFilter;
    private int _activeTab;

    public bool IsFirstTabVisible => ActiveTab == 0;
    public bool IsSecondTabVisible => ActiveTab == 1;

    public string SelectedFilter
    {
      get => _selectedFilter;
      set => SetProperty(ref _selectedFilter, value);
    }

    public int ActiveTab
    {
      get => _activeTab;
      set
      {
        if (SetProperty(ref _activeTab, value))
        {
          OnPropertyChanged(nameof(IsFirstTabVisible));
          OnPropertyChanged(nameof(IsSecondTabVisible));
        }
      }
    }

    public ObservableCollection<string> FilterOptions { get; } = new ObservableCollection<string>
        {
            "Ngày gần đây nhất",
            "Tên người tập",
            "Loại hình tập luyện"
        };

    public TrainingPageViewModel ()
    {
            
        _userList = new ObservableCollection<GetTrainingDetailDto>
        {
            new GetTrainingDetailDto
            {
                Trainee = new GetUserDto
                {
                    UserName = "Nguyễn Thanh Phong",
                    Job = "Sinh viên",
                    AvatarUrl = "pt3.png",
                    DateOfBirth = new DateTime(2003,9,21)
                },
                Slots = new ObservableCollection<GetSlotDto>
                {
                    new GetSlotDto{StartTime = new DateTime(2024,11,1,7,0,0), EndTime = new DateTime(2024,11,1,9,0,0)},
                    new GetSlotDto{StartTime = new DateTime(2024,11,2,7,0,0), EndTime = new DateTime(2024,11,2,9,0,0)},
                    new GetSlotDto{StartTime = new DateTime(2024,11,30,7,0,0), EndTime = new DateTime(2024,11,30,9,0,0)}
                }

                
            },
            new GetTrainingDetailDto
            {

                Trainee = new GetUserDto
                {
                    UserName = "Nguyễn Văn A",
                    Job = "Sinh viên",
                    AvatarUrl = "pt1.png",
                    DateOfBirth = new DateTime(2003,9,21)
                },
                Slots = new ObservableCollection<GetSlotDto>
                {
                    new GetSlotDto{StartTime = new DateTime(2024,11,1,7,0,0), EndTime = new DateTime(2024,11,1,9,0,0)},
                    new GetSlotDto{StartTime = new DateTime(2024,11,2,7,0,0), EndTime = new DateTime(2024,11,2,9,0,0)},
                    new GetSlotDto{StartTime = new DateTime(2024,11,30,7,0,0), EndTime = new DateTime(2024,11,30,9,0,0)}
                }
            },
            new GetTrainingDetailDto
            {
                Trainee = new GetUserDto
                {
                    UserName = "Nguyễn Văn C",
                    Job = "Nhân viên văn phòng",
                    AvatarUrl = "pt2",
                    DateOfBirth = new DateTime(2003,9,21)
                },
                Slots = new ObservableCollection<GetSlotDto>
                {
                    new GetSlotDto{StartTime = new DateTime(2024,11,1,7,0,0), EndTime = new DateTime(2024,11,1,9,0,0)},
                    new GetSlotDto{StartTime = new DateTime(2024,11,2,7,0,0), EndTime = new DateTime(2024,11,2,9,0,0)},
                    new GetSlotDto{StartTime = new DateTime(2024,11,30,7,0,0), EndTime = new DateTime(2024,11,30,9,0,0)}
                }
            }

        };
        ActiveTab = 0;
    }

    // RelayCommand to select tabs
    [RelayCommand]
    private void SelectTab (object parameter)
    {
      if (int.TryParse(parameter?.ToString(), out int tab))
      {
        ActiveTab = tab;
      }
    }
  }
}
