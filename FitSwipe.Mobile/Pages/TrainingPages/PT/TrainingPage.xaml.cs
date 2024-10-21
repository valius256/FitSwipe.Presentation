using FitSwipe.Mobile.Pages.SchedulePages;
using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Utils;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class TrainingPage : ContentPage
{
    public TrainingPageViewModel ViewModel { get; set; }

    public TrainingPage()
	{
		InitializeComponent();
		ViewModel = new TrainingPageViewModel(loadingDialog);
        FirstSetup();
    }
    public async void FirstSetup()
    {
        //pageContent.IsVisible = false;
        ViewModel.MyTrainingFlag = false;
        ViewModel.RequestedTrainingFlag = true;
        await ViewModel.FetchData();
        await ViewModel.HandleSwitchTab();
        //pageContent.IsVisible = true;
        BindingContext = ViewModel;
    }
    private void ImageButton_Pressed (object sender, EventArgs e)
    {

        if (sender is ImageButton button)
        {
            button.ScaleTo(1.12, 100); // Scale up when pressed
        }
    }

    private void ImageButton_Released (object sender, EventArgs e)
    {

        if (sender is ImageButton button)
        {
            button.ScaleTo(1, 100); // Scale back to normal
        }
    }

    private void pageContent_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (e.ScrollY >= (pageContent.ContentSize.Height - pageContent.Height))
        {
            ViewModel.ScrolledToEnd(e);
        }
    }

    private async void btnMoreInfo_Clicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        if (button != null)
        {
            var training = button.CommandParameter as GetTrainingWithTraineeAndPTDto;
            if (training != null)
            {
                await Navigation.PushModalAsync(new CustomersRequestedSchedulePage(training.Id, ViewModel.FetchData));
            }
        }
    }
    private async void btnAccept_Clicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        if (button != null)
        {
            var training = button.CommandParameter as GetTrainingWithTraineeAndPTDto;
            if (training != null)
            {
                await Navigation.PushModalAsync(new PTAcceptSchedule(training.Id, ViewModel.FetchData));
            }
        }
    }

    private async void btnMoreInfoTrained_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            var training = button.CommandParameter as GetTrainingWithTraineeAndPTDto;
            if (training != null)
            {
                await Navigation.PushModalAsync(new CustomersRequestedSchedulePage(training.Id, ViewModel.FetchData));
            }
        }
    }

    private async void btnDeny_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Từ chối yêu cầu đặt lịch này của học viên", "Bạn có chắc chắn về hành động này", "Có", "Không");
        if (answer)
        {
            var button = sender as ImageButton;
            if (button != null)
            {
                var training = button.CommandParameter as GetTrainingWithTraineeAndPTDto;
                if (training != null)
                {
                    loadingDialog.IsVisible = true;
                    loadingDialog.Message = "Vui lòng chờ...";
                    var token = await SecureStorage.GetAsync("auth_token");
                    if (token == null)
                    {
                        throw new Exception("Lỗi xác thực");
                    }
                    try
                    {
                        await Fetcher.PatchAsync($"api/trainings/{training.Id}/rejecting", new GetSlotDto(), token);
                        await DisplayAlert("Thành công", "Đã từ chối yêu cầu này", "OK");
                        await ViewModel.FetchData();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Lỗi", ex.Message, "OK");
                    }
                    loadingDialog.IsVisible = false;
                }
            }
        }
    }
}