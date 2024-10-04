using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.HomePages;

public partial class SwipeMatchView : ContentPage
{
    private ObservableCollection<GetUserWithTagDto> _items = new ObservableCollection<GetUserWithTagDto>();
    private bool isFetching = false;
    private int CurrentPage = 1;
    private int PageSize = 8;
    private int MaxPage = 1;
    private PTList _ptList;
    private Navbar _navbar;
    public ObservableCollection<GetUserWithTagDto> Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }
    public SwipeMatchView(PTList pTList, Navbar navbar)
    {
        InitializeComponent();
        FetchData();
        _ptList = pTList;
        _navbar = navbar;
        BindingContext = this;
    }

    private void OnCurrentItemChanged(object? sender, CurrentItemChangedEventArgs e)
    {
        //matchView.CurrentItemChanged += OnCurrentItemChanged;
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
                await Fetcher.PostAsync($"api/trainings", new  CreateTrainingDto
                {
                    PTId =  boundItem.FireBaseId,
                    Status = Shared.Enums.TrainingStatus.Matched
                }, token);
                var toast = Toast.Make("Thành công! Bạn đã match với " + boundItem.UserName,ToastDuration.Short);
                await toast.Show();
                Items = Items.Where(u => u.Id != boundItem.Id).ToObservableCollection();
                _ptList.Items = Items.ToObservableCollection();
                _navbar.TrainingFlag = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", "Có sự cố xảy ra. Err : " + ex.Message, "OK");
            }
            loadingDialog.IsVisible = false;
            isFetching = false;
        }
    }
    private async void FetchData()
    {
        matchView.IsVisible = false;
        loadingDialog.IsVisible = true;
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
                AppendList(response.Items);
                MaxPage = (int)Math.Ceiling((double)response.Total / response.Limit);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có sự cố xảy ra. Err : " + ex.Message, "OK");
        }
        loadingDialog.IsVisible = false;
        isFetching = false;
        matchView.IsVisible = true;
    }
    private void AppendList(IList<GetUserWithTagDto> users)
    {
        foreach (var user in users)
        {
            Items.Add(user);
        }
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
                AppendList(response.Items);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có sự cố xảy ra. Err : " + ex.Message, "OK");
        }
        isFetching = false;
    }
    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void matchView_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        if (!isFetching)
        {
            if (CurrentPage < MaxPage)
            {
                CurrentPage++;
                await ContinueFetchingData();
            }
        } 
    }
}