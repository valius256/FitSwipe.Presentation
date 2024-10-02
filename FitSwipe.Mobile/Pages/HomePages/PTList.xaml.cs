using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Utils;
using Mapster;
using Syncfusion.Maui.Inputs;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.HomePages;

public partial class PTList : ContentPage
{
    private ObservableCollection<GetUserWithTagDto> _items = new ObservableCollection<GetUserWithTagDto>();
    private bool isFetching = false;
    private int CurrentPage = 1;
    private int PageSize = 8;
    private int MaxPage = 1;
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

    private void btnSwipeMatch_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new SwipeMatchView());
        //await Shell.Current.GoToAsync("//SwipeMatchView");
    }
    private async void FetchData()
    {
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
            Items.Add(user);
            await Task.Delay(300);
        }
    }
    private void btnMatch_Clicked(object sender, EventArgs e)
    {

    }

    private async void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        if (!isFetching && CurrentPage < MaxPage)
        {
            CurrentPage++;
            await ContinueFetchingData();
        }
    }
}