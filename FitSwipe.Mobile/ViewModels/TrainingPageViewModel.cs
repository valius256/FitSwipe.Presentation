using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Utils;
using Mapster;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class TrainingPageViewModel : ObservableObject
  {
    private LoadingDialog _loadingDialog;

    [ObservableProperty]
    private ObservableCollection<GetTrainingWithTraineeAndPTDto> _userList = new();
    private ObservableCollection<GetTrainingWithTraineeAndPTDto> _requestedTraining = new();
    private ObservableCollection<GetTrainingWithTraineeAndPTDto> _myTraining = new();
    public bool RequestedTrainingFlag = true;
    public bool MyTrainingFlag = true;

    private string? _token = string.Empty;
    private GetUserDto? _currentUser;
    private string? _selectedFilter;
    private int _activeTab;
    private bool isFetching = false;
    private int CurrentPage = 1;
    private int PageSize = 5;
    private int MaxPage = 1;
    public bool IsFirstTabVisible => ActiveTab == 0;
    public bool IsSecondTabVisible => ActiveTab == 1;

    public string? SelectedFilter
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

    public TrainingPageViewModel (LoadingDialog loadingDialog)
    {
        _loadingDialog = loadingDialog;
        ActiveTab = 0;
        Setup();
    }
    public async Task HandleSwitchTab()
    {
        if (ActiveTab == 1)
        {
            if (RequestedTrainingFlag)
            {
                await FetchData();
                RequestedTrainingFlag = false;
            }
            _userList.Clear();
            await AppendList(_requestedTraining);
        }
        else if (ActiveTab == 0)
        {
            if (MyTrainingFlag)
            {
                await FetchData();
                    MyTrainingFlag = false;
            }
            _userList.Clear();
            await AppendList(_myTraining);
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
            if (_currentUser == null)
            {
                throw new Exception();
            }
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
    public async Task FetchData()
    {
            _loadingDialog.IsVisible = true;
            _loadingDialog.Message = "Đang lấy dữ liệu...";
            isFetching = true;
            CurrentPage = 1;
            var token = await SecureStorage.GetAsync("auth_token");
            if (token != null)
            {
                try
                {
                    _userList.Clear();
                    string queryStatusString = ActiveTab == 0 ? "Filter.TrainingStatuses=2&Filter.TrainingStatuses=3&Filter.TrainingStatuses=4" : "Filter.TrainingStatuses=1";
                    var response = await Fetcher.GetAsync<PagedResult<GetTrainingWithTraineeAndPTDto>>($"api/trainings?&Filter.PTId={_currentUser?.Id}&limit={PageSize}&page={CurrentPage}&{queryStatusString}", token);
                    if (response == null)
                    {
                        throw new Exception("Vui lòng thử lại sau");
                    }
                    var list = response.Items.Adapt<ObservableCollection<GetTrainingWithTraineeAndPTDto>>();
                    if (ActiveTab == 0)
                    {
                        _myTraining = list.ToObservableCollection();
                    }
                    else
                    {
                        _requestedTraining = list.ToObservableCollection();
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
            _loadingDialog.IsVisible = false;
            isFetching = false;
    }
    private async Task ContinueFetchData()
    {
            isFetching = true;
            CurrentPage = 1;
            var token = await SecureStorage.GetAsync("auth_token");
            if (token != null)
            {
                try
                {
                    string queryStatusString = ActiveTab == 0 ? "Filter.TrainingStatuses=2&Filter.TrainingStatuses=3&Filter.TrainingStatuses=4" : "Filter.TrainingStatuses=1";
                    var response = await Fetcher.GetAsync<PagedResult<GetTrainingWithTraineeAndPTDto>>($"api/trainings?&Filter.TraineeId={_currentUser?.Id}&limit={PageSize}&page={CurrentPage}&{queryStatusString}", token);
                    if (response == null)
                    {
                        throw new Exception("Vui lòng thử lại sau");
                    }
                    var list = response.Items.Adapt<ObservableCollection<GetTrainingWithTraineeAndPTDto>>();
                    foreach (var item in list)
                    {
                        if (ActiveTab == 0)
                        {
                            _myTraining.Add(item);
                        }
                        else
                        {
                            _requestedTraining.Add(item);
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
    public async void ScrolledToEnd(object parameter)
    {
        if (!isFetching && CurrentPage < MaxPage)
        {
            CurrentPage++;
            await ContinueFetchData();
        }
    }
    // RelayCommand to select tabs
    [RelayCommand]
    private async void SelectTab (object parameter)
    {
      if (int.TryParse(parameter?.ToString(), out int tab))
      {
        ActiveTab = tab;
        await HandleSwitchTab();

      }
    }
  }
}
