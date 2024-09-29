using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.MockData;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class TrainingPageViewModel : ObservableObject
  {
    [ObservableProperty]
    private ObservableCollection<User> userList = new();

    [ObservableProperty]
    private User user = new();

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
      userList = new ObservableCollection<User>
                {
                    new User
                    {
                        Name = "Nguyễn Thanh Phong",
                        Occupation = "Sinh viên",
                        DoB = "21/09/2003",
                        DurationPerSection = 21,
                        PracticeTime = 20
                    },

                    new User
                    {
                        Name = "Nguyễn Văn A",
                        Occupation = "Sinh viên",
                        DoB = "21/09/2003",
                        DurationPerSection = 21,
                        PracticeTime = 20
                    },
                    new User
                    {
                        Name = "Nguyễn Văn B",
                        Occupation = "Nhân viên văn phòng",
                        DoB = "21/09/2003",
                        DurationPerSection = 21,
                        PracticeTime = 20
                    },
                    new User
                    {
                        Name = "Nguyễn Văn C",
                        Occupation = "Nhân viên văn phòng",
                        DoB = "21/09/2003",
                        DurationPerSection = 21,
                        PracticeTime = 20
                    },
                    new User
                    {
                        Name = "Nguyễn Văn D",
                        Occupation = "Nhân viên văn phòng",
                        DoB = "21/09/2003",
                        DurationPerSection = 21,
                        PracticeTime = 20
                    },
                    new User
                    {
                        Name = "Nguyễn Văn E",
                        Occupation = "Nhân viên văn phòng",
                        DoB = "21/09/2003",
                        DurationPerSection = 21,
                        PracticeTime = 20
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
