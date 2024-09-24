using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.MockData;
using System.Collections.ObjectModel;
using System.Diagnostics;

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

        public Command<int> SelectTabCommand { get; }
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
                SetProperty(ref _activeTab, value);
                OnPropertyChanged(nameof(IsFirstTabVisible));
                OnPropertyChanged(nameof(IsSecondTabVisible));
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
                        Email = "nthanhphong941@gmail.com"
                    },

                    new User
                    {
                        Name = "Nguyễn Văn A",
                        Email = "nguynthanhfont@gmail.com"
                    }
                };
            ActiveTab = 0; // Default to first tab
            SelectTabCommand = new Command<int>(SelectTab);
        }

        private void SelectTab (int tab)
        {
            Debug.WriteLine($"Command executed for tab: {tab}");
            ActiveTab = tab;
        }
    }
}
