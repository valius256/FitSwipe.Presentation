using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Mobile.Controls;
using FitSwipe.Mobile.Pages.ProfilePages;
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Utils;
using Mapster;
using Syncfusion.Maui.Inputs;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.HomePages;
[QueryProperty(nameof(PassedFlag), "flag")]
public partial class PTList : ContentPage
{
    private ObservableCollection<GetUserWithTagDto> _items = new ObservableCollection<GetUserWithTagDto>();
    private GetUserDetailDto _loginedUser = new();
    private bool isFetching = false;
    private int CurrentPage = 1;
    private int PageSize = 8;
    private int MaxPage = 1;
    public bool PassedFlag { get; set; } = false;

    public ObservableCollection<GetUserWithTagDto> Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }
    public PTList()
	{
        InitializeComponent();
        FetchData();
        BindingContext = this;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (PassedFlag)
        {
            Items.Clear();
            FetchData();
            PassedFlag = false;
        }
    }

    private void btnSwipeMatch_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new SwipeMatchView(this, navbar, _loginedUser));
        //await Shell.Current.GoToAsync("//SwipeMatchView");
    }
    /// <summary>
    /// Fetch the data for the first time
    /// </summary>
    private async void FetchData()
    {
        CurrentPage = 1;
        loadingDialog.IsVisible = true;
        loadingDialog.Message = "Đang chuẩn bị danh sách PT cho bạn...";
        isFetching = true;
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token == null)
            {
                throw new Exception("Có sự cố xảy ra. Vui lòng đăng nhập lại");
            }
            var loginedUserId = await SecureStorage.GetAsync("loginedUserId");
            if (loginedUserId == null)
            {
                throw new Exception("Lỗi xác thực");
            }
            var loginedUser = await Fetcher.GetAsync<GetUserDetailDto>($"api/users/{loginedUserId}/detail");
            if (loginedUser != null)
            {
                _loginedUser = loginedUser;
            }
            var response = await Fetcher.GetAsync<PagedResult<GetUserWithTagDto>>($"api/users/match-ordered?Filter.Roles=1&page={CurrentPage}&limit={PageSize}", token);
            if (response != null)
            {
                await AppendList(response.Items);
                MaxPage = (int)  Math.Ceiling((double) response.Total / response.Limit);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi","Có sự cố xảy ra. Err : " + ex.Message,"OK");
        }
        loadingDialog.IsVisible = false;
        isFetching = false;
    }
    private async Task ContinueFetchingData()
    {
        isFetching = true;
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token == null)
            {
                throw new Exception("Có sự cố xảy ra. Vui lòng đăng nhập lại");
            }
            var response = await Fetcher.GetAsync<PagedResult<GetUserWithTagDto>>($"api/users/match-ordered?Filter.Roles=1&page={CurrentPage}&limit={PageSize}", token);
            if (response != null)
            {
                await AppendList(response.Items);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có sự cố xảy ra. Err : " + ex.Message, "OK");
        }
        isFetching = false;
    }
    private async Task AppendList(IList<GetUserWithTagDto> users)
    {
        foreach (var user in users)
        {
            user.DistanceInKm = Helper.CalculateDistance(user.Latitude ?? 0, user.Longitude ?? 0, _loginedUser.Latitude ?? 0, _loginedUser.Longitude ?? 0);
            Items.Add(user);
            await Task.Delay(300);
        }
    }
    private async void btnMatch_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        // Retrieve the bound object via CommandParameter
        var boundItem = button?.CommandParameter as GetUserWithTagDto;

        if (boundItem != null)
        {
            loadingDialog.IsVisible = true;
            loadingDialog.Message = "Đang thực hiện...";
            isFetching = true;
            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (token == null)
                {
                    throw new Exception("Có sự cố xảy ra. Vui lòng đăng nhập lại");
                }
                await Fetcher.PostAsync($"api/trainings", new CreateTrainingDto
                {
                    PTId = boundItem.FireBaseId,
                    Status = Shared.Enums.TrainingStatus.Matched
                }, token);
                var toast = Toast.Make("Thành công! Bạn đã match với " + boundItem.UserName, ToastDuration.Short);
                await toast.Show();
                Items = Items.Where(u => u.Id != boundItem.Id).ToObservableCollection();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", "Có sự cố xảy ra. Err : " + ex.Message, "OK");
            }
            loadingDialog.IsVisible = false;
            isFetching = false;
            navbar.TrainingFlag = true;
        }
    }

    private async void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        if (!isFetching && CurrentPage < MaxPage)
        {
            CurrentPage++;
            await ContinueFetchingData();
        }
    }

    private async void tapAvatar_Tapped(object sender, TappedEventArgs e)
    {
        var frame = sender as Frame;
        if (frame != null && frame.GestureRecognizers.Count > 0)
        {
            var tapGuesture = frame.GestureRecognizers[0] as TapGestureRecognizer;
            if (tapGuesture != null)
            {
                var boundItem = tapGuesture.CommandParameter as GetUserWithTagDto;
                if (boundItem != null && !loadingDialog.IsVisible)
                {
                    loadingDialog.IsVisible = true;
                    loadingDialog.Message = "Vui lòng chờ...";
                    await Navigation.PushModalAsync(new PTProfilePage(boundItem.FireBaseId));
                    loadingDialog.IsVisible = false;

                }
            }
        }
    }
}