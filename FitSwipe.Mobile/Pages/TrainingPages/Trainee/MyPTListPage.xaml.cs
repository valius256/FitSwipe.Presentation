using CommunityToolkit.Mvvm.Input;
using FitSwipe.Mobile.Pages.FeedbackPages;
using FitSwipe.Mobile.Pages.ProfilePages;
using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Utils;

namespace FitSwipe.Mobile.Pages.TrainingPages;

[QueryProperty(nameof(PassedFlag), "flag")]
public partial class MyPTListPage : ContentPage
{
    public bool PassedFlag { get; set; } = false;
    public MyPTListPageViewModel ViewModel { get; set; }
    private GetTrainingDetailDto? CurrentTrainingDetail { get; set; }
    private string _token = string.Empty;
    public MyPTListPage()
    {
        InitializeComponent();
        ViewModel = new MyPTListPageViewModel();
        ViewModel.loadingDialog = loadingDialog;
        ViewModel.Navbar = navbar;
        //FirstSetup();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var currentToken = await SecureStorage.GetAsync("auth_token") ?? string.Empty;
        if (Helper.CheckTokenChanged(_token, currentToken))
        {
            
            FirstSetup();
            return;
        }
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
        ViewModel.MatchedFlag = (ViewModel.ActiveTab == 1);
        ViewModel.BookedFlag = (ViewModel.ActiveTab == 0);
        await ViewModel.FetchData();
        await ViewModel.HandleSwitchTab();
        //pageContent.IsVisible = true;

        //GET CURRENT TRAINING
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token == null) throw new Exception();
            _token = token;
            var trainingResult = await Fetcher.GetAsync<GetTrainingDetailDto>("api/trainings/current-training", _token);
            CurrentTrainingDetail = trainingResult;
        }
        catch
        {
            CurrentTrainingDetail = null;
        }


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
                        await Fetcher.DeleteAsync($"api/trainings/{boundItem.Id}/cancel", _token);
                        ViewModel.RemoveFromList(boundItem);                      
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

    //private void pageContent_Scrolled(object sender, ScrolledEventArgs e)
    //{
        
    //}

    private async void btnBooking_Clicked(object sender, EventArgs e)
    {   
        
        var button = sender as Button;
        if (button != null)
        {
            var boundItem = button.CommandParameter as GetTrainingWithTraineeAndPTDto;
            if (boundItem != null)
            {
                if (CurrentTrainingDetail != null && boundItem.Status == Shared.Enums.TrainingStatus.Matched)
                {
                    await DisplayAlert("Thất bại", "Bạn hiện đang huấn luyện bởi 1 PT khác. Bạn có thể hủy lịch với họ để đặt PT mới", "OK");
                    return;
                }
                await Navigation.PushModalAsync(new PTScheduleBookingView
                {
                    Training = boundItem,
                    MyPTListPage = this
                });
            }
        }
    }

    private async void btnRate_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            var boundItem = button.CommandParameter as GetTrainingWithTraineeAndPTDto;
            if (boundItem != null)
            {
                await Navigation.PushModalAsync(new FeeedbackPage(boundItem, ViewModel));
            }
        }
    }

    private void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        ViewModel.ScrolledToEnd(sender);
    }

    private void tapPTAvatar_Tapped(object sender, TappedEventArgs e)
    {
        var border = sender as Border;
        if (border != null && border.GestureRecognizers.Count > 0)
        {
            var tapGesture = border.GestureRecognizers[0] as TapGestureRecognizer;
            if (tapGesture != null)
            {
                var boundItem = tapGesture.CommandParameter as GetTrainingWithTraineeAndPTDto;
                if (boundItem != null)
                {
                    Navigation.PushModalAsync(new PTProfilePage(boundItem.PTId));
                }
            }
        }
    }
}