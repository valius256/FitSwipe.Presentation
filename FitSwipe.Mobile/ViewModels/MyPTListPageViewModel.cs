using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.Controls;
using FitSwipe.Mobile.MockData;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using Mapster;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.ViewModels
{
    public partial class MyPTListPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<GetTrainingWithTraineeAndPTDto> _userList = new();

        private ObservableCollection<GetTrainingWithTraineeAndPTDto> _matchedTraining = new();
        private ObservableCollection<GetTrainingWithTraineeAndPTDto> _bookedTraining = new();
        public bool MatchedFlag = true;
        public bool BookedFlag = true;

        private string _selectedFilter = string.Empty;
        private int _activeTab;
        private bool isFetching = false;
        private int CurrentPage = 1;
        private int PageSize = 5;
        private int MaxPage = 1;
        private string? _token = string.Empty;
        private GetUserDto? _currentUser;
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

        public LoadingDialog loadingDialog { get; set; } = new LoadingDialog();
        public Navbar Navbar { get; set; } = new Navbar();

        public ObservableCollection<string> FilterOptions { get; } = new ObservableCollection<string>
        {
            "Ngày gần đây nhất",
            "Tên người tập",
            "Loại hình tập luyện"
        };

        public MyPTListPageViewModel()
        {
            ActiveTab = 0;
            Setup();
        }
        private async void Setup()
        {
            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (token == null)
                {
                    throw new Exception();
                }
                _token = token;
                _currentUser = await Shortcut.GetLoginedUser(token);
            }
            catch
            {
                if (Application.Current != null && Application.Current.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Lỗi xác thực, vui lòng đăng nhập lại", "OK");
                }
                await Shell.Current.GoToAsync("//SignIn");
            }
            
        }
        public void RemoveFromList(GetTrainingWithTraineeAndPTDto training)
        {
            _userList.Remove(training);
        }
        public async Task HandleSwitchTab()
        {
            if (ActiveTab == 0)
            {
                if (MatchedFlag)
                {
                    await FetchData();
                    MatchedFlag = false;
                }
                _userList.Clear();
                await AppendList(_matchedTraining);
            }
            else if (ActiveTab == 1)
            {
                if (BookedFlag)
                {
                    await FetchData();
                    BookedFlag = false;
                }
                _userList.Clear();
                await AppendList(_bookedTraining);
            }
        }
        public async Task FetchData()
        {
            loadingDialog.IsVisible = true;
            loadingDialog.Message = "Đang lấy dữ liệu...";
            isFetching = true;
            CurrentPage = 1;
            var token = await SecureStorage.GetAsync("auth_token");
            if (token != null)
            {
                try
                {
                    _userList.Clear();
                    string queryStatusString = ActiveTab == 0 ? "Filter.TrainingStatuses=0" : "Filter.TrainingStatuses=1&Filter.TrainingStatuses=2&Filter.TrainingStatuses=3&Filter.TrainingStatuses=4&Filter.TrainingStatuses=6";
                    var response = await Fetcher.GetAsync<PagedResult<GetTrainingWithTraineeAndPTDto>>($"api/trainings?&Filter.TraineeId={_currentUser?.Id}&limit={PageSize}&page={CurrentPage}&{queryStatusString}", token);
                    if (response == null)
                    {
                        throw new Exception("Vui lòng thử lại sau");
                    }
                    var list = response.Items.Adapt<ObservableCollection<GetTrainingWithTraineeAndPTDto>>();
                    if (ActiveTab == 0)
                    {
                        _matchedTraining = list.ToObservableCollection();
                    } else
                    {
                        _bookedTraining = list.ToObservableCollection();
                        GetStatuses();
                    }
                    MaxPage = response.Total;
                }
                catch (Exception ex)
                {
                    if (Application.Current != null && Application.Current.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                    }
                }
            }
            loadingDialog.IsVisible = false;
            isFetching = false;
        }
        private async Task ContinueFetchingData()
        {
            isFetching = true;
            var token = await SecureStorage.GetAsync("auth_token");
            if (token != null)
            {
                try
                {
                    string queryStatusString = ActiveTab == 0 ? "Filter.TrainingStatuses=0" : "Filter.TrainingStatuses=1&Filter.TrainingStatuses=2&Filter.TrainingStatuses=3&Filter.TrainingStatuses=4&Filter.TrainingStatuses=6";
                    var response = await Fetcher.GetAsync<PagedResult<GetTrainingWithTraineeAndPTDto>>($"api/trainings?limit={PageSize}&page={CurrentPage}&{queryStatusString}", token);
                    if (response == null)
                    {
                        throw new Exception("Vui lòng thử lại sau");
                    }
                    var list = response.Items.Adapt<ObservableCollection<GetTrainingWithTraineeAndPTDto>>();
                    foreach (var item in list)
                    {
                        if (ActiveTab == 0)
                        {
                            _matchedTraining.Add(item);
                        }
                        else
                        {
                            _bookedTraining.Add(item);
                            GetStatuses();
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    if (Application.Current != null && Application.Current.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                    }
                }
            }           
            isFetching = false;
        }

        private void GetStatuses()
        {
            foreach (var item in _bookedTraining)
            {
                if (item.Status == TrainingStatus.Rejected)
                {
                    item.StatusString = "Đã bị từ chối";
                    item.StatusColor = "#ff1205";
                }
                if (item.Status == TrainingStatus.Pending)
                {
                    item.StatusString = "Đang chờ duyệt lịch";
                    item.StatusColor = "#969595";
                }
                if (item.Status == TrainingStatus.NotStarted)
                {
                    item.StatusString = "Sắp bắt đầu";
                    item.StatusColor = "#52BB00";
                }
                if (item.Status == TrainingStatus.OnGoing)
                {
                    item.StatusString = "Đang diễn ra";
                    item.StatusColor = "#d18b08";
                }
                if (item.Status == TrainingStatus.Finished)
                {
                    item.StatusString = "Đã hoàn tất";
                    item.StatusColor = "#032b8a";
                }
                if (item.Status == TrainingStatus.Disabled)
                {
                    item.StatusString = "Đã vô hiệu hóa";
                    item.StatusColor = "#000000";
                }
            }
        }
        private async Task AppendList(IList<GetTrainingWithTraineeAndPTDto> trainings)
        {
            foreach (var training in trainings)
            {
                _userList.Add(training);
                await Task.Delay(100);
            }
        }
        public async void ScrolledToEnd(object parameter)
        {
            if (!isFetching && CurrentPage < MaxPage)
            {
                CurrentPage++;
                await ContinueFetchingData();
            }
        }

        [RelayCommand]
        private async void SelectTab(object parameter)
        {
            if (int.TryParse(parameter?.ToString(), out int tab))
            {
                ActiveTab = tab;
                await HandleSwitchTab();
            }
        }
        
    }
}
