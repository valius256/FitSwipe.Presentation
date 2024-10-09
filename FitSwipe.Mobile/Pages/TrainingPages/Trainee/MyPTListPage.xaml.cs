using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Utils;

namespace FitSwipe.Mobile.Pages.TrainingPages;

[QueryProperty(nameof(PassedFlag), "flag")]
public partial class MyPTListPage : ContentPage
{
    public bool PassedFlag { get; set; } = false;
    public MyPTListPageViewModel ViewModel { get; set; }
    public MyPTListPage()
    {
        InitializeComponent();
        ViewModel = new MyPTListPageViewModel();
        ViewModel.loadingDialog = loadingDialog;
        ViewModel.Navbar = navbar;
        FirstSetup();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (PassedFlag)
        {
            ViewModel.MatchedFlag = false;
            ViewModel.BookedFlag = true;
            Setup();
            PassedFlag = false;
        }
    }
    public async void Setup()
    {
        await ViewModel.FetchData();
        await ViewModel.HandleSwitchTab();
        //BindingContext = ViewModel;
    }
    public async void FirstSetup()
    {
        //pageContent.IsVisible = false;
        ViewModel.MatchedFlag = false;
        ViewModel.BookedFlag = true;
        await ViewModel.FetchData();
        await ViewModel.HandleSwitchTab();
        //pageContent.IsVisible = true;
        BindingContext = ViewModel;
    }

    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            //loadingDialog.Message = "Đang thực hiện...";
            //loadingDialog.IsVisible = true;
            var answer = await DisplayAlert("Xóa PT này khỏi danh sách match","Bạn có chắc chắn về hành động này chứ?","Có","Không");
            if (answer)
            {
                var boundItem = button.CommandParameter as GetTrainingWithTraineeAndPTDto;
                if (boundItem != null)
                {
                    try
                    {
                        var token = await SecureStorage.GetAsync("auth_token");
                        if (token != null)
                        {
                            await Fetcher.DeleteAsync($"api/trainings/{boundItem.Id}/cancel", token);
                            ViewModel.RemoveFromList(boundItem);
                        }
                        ViewModel.MatchedFlag = true;
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                    }
                }
                navbar.HomeFlag = true;
                //loadingDialog.IsVisible = false;
            }            
        }
    }

    private void pageContent_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (e.ScrollY >= (pageContent.ContentSize.Height - pageContent.Height))
        {
            ViewModel.ScrolledToEnd(e);
        }
    }

    //THIS SHIT IS STILL CAUSING CRASH WHILE RUN WITHOUT DEBUGGING
    private async void btnBooking_Clicked(object sender, EventArgs e)
    {   try
        {
            var button = sender as Button;
            if (button != null)
            {
                var boundItem = button.CommandParameter as GetTrainingWithTraineeAndPTDto;
                if (boundItem != null)
                {
                    await Navigation.PushModalAsync(new PTScheduleBookingView
                    {
                        Training = boundItem,
                        MyPTListPage = this
                    });
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message + " \n" + ex.StackTrace, "OK");
        }
    }
}