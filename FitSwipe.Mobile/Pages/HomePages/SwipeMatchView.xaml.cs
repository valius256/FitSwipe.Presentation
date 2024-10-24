using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Mobile.Controls;
using FitSwipe.Mobile.Extensions;
using FitSwipe.Mobile.Pages.ProfilePages;
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Paging;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using Microsoft.Maui.Controls;
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
    private GetUserDetailDto _loginedUser;
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
    public SwipeMatchView(PTList pTList, Navbar navbar, GetUserDetailDto user)
    {
        InitializeComponent();
        FetchData();
        _ptList = pTList;
        _navbar = navbar;
        _loginedUser = user;
        BindingContext = this;
    }

    private async void OnCurrentItemChanged(object? sender, CurrentItemChangedEventArgs e)
    {
        //matchView.CurrentItemChanged += OnCurrentItemChanged;
        if (e.CurrentItem != null)
        {
            var carouselView = sender as CarouselView;
            if (carouselView != null)
            {
                // Get the binding context of the current item
                var currentItem = e.CurrentItem;

                // Loop through all the visible views to find the one that matches the current item
                var views = carouselView.VisibleViews.ToList();
                foreach (var visibleView in views)
                {
                    if (visibleView.BindingContext == currentItem)
                    {
                        // We found the view that corresponds to the current item
                        var flyoutLayout = visibleView.FindByName<StackLayout>("animateFlyout");
                        var viplayout = visibleView.FindByName<HorizontalStackLayout>("viptitle");

                        var tasks = new List<Task>();

                        if (flyoutLayout != null)
                        {
                            // Set initial position of the flyout off-screen
                            flyoutLayout.TranslationX = -flyoutLayout.Width;

                            // Add flyout animation to the task list
                            tasks.Add(flyoutLayout.TranslateTo(0, 0, 500, Easing.CubicOut)); // 500ms animation duration
                        }

                        if (viplayout != null)
                        {
                            // Set initial position of the flyout off-screen
                            viplayout.TranslationX = viplayout.Width;

                            // Add viplayout animation to the task list
                            tasks.Add(viplayout.TranslateTo(0, 0, 500, Easing.CubicIn)); // 500ms animation duration
                        }

                        // Run both animations simultaneously
                        await Task.WhenAll(tasks);

                    }
                }
            }
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
                await Fetcher.PostAsync($"api/trainings", new  CreateTrainingDto
                {
                    PTId =  boundItem.FireBaseId,
                    Status = Shared.Enums.TrainingStatus.Matched
                }, token);
                var toast = Toast.Make("Thành công! Bạn đã match với " + boundItem.UserName,ToastDuration.Short);
                await toast.Show();
                Items.Remove(boundItem);
                _ptList.Items.Remove(boundItem);
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
            user.DistanceInKm = Helper.CalculateDistance(user.Latitude ?? 0, user.Longitude ?? 0, _loginedUser.Latitude ?? 0, _loginedUser.Longitude ?? 0);

            var commonHobbies = user.Tags.Where(t => _loginedUser.Tags.FirstOrDefault(loginedUserTag => loginedUserTag.Id == t.Id) != null && t.TagType == TagType.Hobby).ToList();
            var commonTrainingTypes = user.Tags.Where(t => _loginedUser.Tags.FirstOrDefault(loginedUserTag => loginedUserTag.Id == t.Id) != null && t.TagType == TagType.TrainingType).ToList();
            var commonTarget = user.Tags.Where(t => _loginedUser.Tags.FirstOrDefault(loginedUserTag => loginedUserTag.Id == t.Id) != null && t.TagType == TagType.Target).ToList();
            var commonPTTaste = user.Tags.Where(t => _loginedUser.Tags.FirstOrDefault(loginedUserTag => loginedUserTag.Id == t.Id) != null && t.TagType == TagType.PTTaste).ToList();
            var commonSelfDescription = user.Tags.Where(t => _loginedUser.Tags.FirstOrDefault(loginedUserTag => loginedUserTag.Id == t.Id) != null && t.TagType == TagType.SelfDescription).ToList();
            
            for (int i = 0; i < Math.Min(3, commonHobbies.Count); i++)
            {
                user.CommonHobby += commonHobbies[i].Name;
                if (i < commonHobbies.Count - 1)
                {
                    user.CommonHobby += ", ";
                }
                if (i == 2)
                {
                    if (commonHobbies.Count > 3)
                    {
                        user.CommonHobby += "(+" + (commonHobbies.Count - 3) + ")";
                    }
                } 
            }
            for (int i = 0; i < Math.Min(3, commonTarget.Count); i++)
            {
                user.CommonTarget += commonTarget[i].Name;
                if (i < commonTarget.Count - 1)
                {
                    user.CommonTarget += ", ";
                }
                if (i == 2)
                {
                    if (commonTarget.Count > 3)
                    {
                        user.CommonTarget += "(+" + (commonTarget.Count - 3) + ")";
                    }
                }
            }
            for (int i = 0; i < Math.Min(3, commonTrainingTypes.Count); i++)
            {
                user.CommonTrainingType += commonTrainingTypes[i].Name;
                if (i < commonTrainingTypes.Count - 1)
                {
                    user.CommonTrainingType += ", ";
                }
                if (i == 2)
                {
                    if (commonTrainingTypes.Count > 3)
                    {
                        user.CommonTrainingType += "(+" + (commonTrainingTypes.Count - 3) + ")";
                    }
                }
            }
            for (int i = 0; i < Math.Min(3,  commonPTTaste.Count); i++)
            {
                user.CommonPTTaste += commonPTTaste[i].Name;
                if (i < commonPTTaste.Count - 1)
                {
                    user.CommonPTTaste += ", ";
                }
                if (i == 2)
                {
                    if (commonPTTaste.Count > 3)
                    {
                        user.CommonPTTaste += "(+" + (commonPTTaste.Count - 3) + ")";
                    }
                }
            }
            for (int i = 0; i < Math.Min(3, commonSelfDescription.Count); i++)
            {
                user.CommonSelfDescription += commonSelfDescription[i].Name;
                if (i < commonSelfDescription.Count - 1)
                {
                    user.CommonSelfDescription += ", ";
                }
                if (i == 2)
                {
                    if (commonSelfDescription.Count > 3)
                    {
                        user.CommonSelfDescription += "(+" + (commonSelfDescription.Count - 3) + ")";
                    }
                }
            }
            //user.CommonHobby 
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

    private async void btnName_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            var boundItem = button.CommandParameter as GetUserWithTagDto;
            if (boundItem != null && !loadingDialog.IsVisible)
            {
                loadingDialog.IsVisible = true;
                loadingDialog.Message = "Vui lòng chờ...";
                await Navigation.PushModalAsync(new PTProfilePage(boundItem.FireBaseId));
                loadingDialog.IsVisible = false;

            }
        }
    }

    private void btnRemove_Clicked(object sender, EventArgs e)
    {

    }
}