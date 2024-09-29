using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.MockData;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class MyPTListPageViewModel : ObservableObject
  {
    [ObservableProperty]
    private ObservableCollection<GetMatchedPTDto> _userList = new();

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

    public MyPTListPageViewModel ()
    {
        var tags = new ObservableCollection<GetTagDto>
        {
            new GetTagDto{Name = "Đá bóng", TagType = TagType.Hobby},
            new GetTagDto{Name = "Đá banh", TagType = TagType.Hobby},
            new GetTagDto{Name = "Bóng chuyền", TagType = TagType.Hobby},
            new GetTagDto{Name = "Bóng rổ", TagType = TagType.Hobby},
            new GetTagDto{Name = "Cardio", TagType = TagType.TrainingType},
            new GetTagDto{Name = "Yoga", TagType = TagType.TrainingType},
        };

        _userList = new ObservableCollection<GetMatchedPTDto>
        {
            new GetMatchedPTDto
            {
                PT = new GetUserWithTagDto
                {
                    UserName = "Nguyễn Thanh Phong",
                    Ward = "Phướng 1",
                    District = "Quận 1",
                    City = "TPHCM",
                    Bio = "*Some interesting and inspiring speech*",
                    DateOfBirth = new DateTime(2003,9,21),
                    AvatarUrl = "pt2",
                    Tags = tags
                },
                Slots = new ObservableCollection<GetSlotDto>
                {
                    new GetSlotDto{StartTime = new DateTime(2024,11,1,7,0,0), EndTime = new DateTime(2024,11,1,9,0,0)},
                    new GetSlotDto{StartTime = new DateTime(2024,11,2,7,0,0), EndTime = new DateTime(2024,11,2,9,0,0)},
                    new GetSlotDto{StartTime = new DateTime(2024,11,30,7,0,0), EndTime = new DateTime(2024,11,30,9,0,0)}
                }


            },
            new GetMatchedPTDto
            {

                PT = new GetUserWithTagDto
                {
                    UserName = "Nguyễn Văn A",
                    Ward = "Phướng 1",
                    District = "Quận 1",
                    City = "TPHCM",
                    Bio = "*Some interesting and inspiring speech*",
                    AvatarUrl = "pt1",
                    DateOfBirth = new DateTime(2003,9,21),
                    Tags = tags
                },
                Slots = new ObservableCollection<GetSlotDto>
                {
                    new GetSlotDto{StartTime = new DateTime(2024,11,1,7,0,0), EndTime = new DateTime(2024,11,1,9,0,0)},
                    new GetSlotDto{StartTime = new DateTime(2024,11,2,7,0,0), EndTime = new DateTime(2024,11,2,9,0,0)},
                    new GetSlotDto{StartTime = new DateTime(2024,11,30,7,0,0), EndTime = new DateTime(2024,11,30,9,0,0)}
                }
            },
            new GetMatchedPTDto
            {
                PT = new GetUserWithTagDto
                {
                    UserName = "Nguyễn Văn C",
                    Ward = "Phướng 1",
                    District = "Quận 1",
                    City = "TPHCM",
                    Bio = "*Some interesting and inspiring speech*",
                    DateOfBirth = new DateTime(2003,9,21),
                    AvatarUrl = "pt3",
                    Tags = tags
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
