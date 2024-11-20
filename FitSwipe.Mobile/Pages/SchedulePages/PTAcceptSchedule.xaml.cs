using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class PTAcceptSchedule : ContentPage
{
    private Guid _trainingId;
    private Func<Task> _refresh;
    private GetTrainingDetailDto _trainingDetail { get; set; } = new GetTrainingDetailDto();
    public GetTrainingDetailDto TrainingDetail
    {
        get => _trainingDetail;
        set
        {
            _trainingDetail = value;
            OnPropertyChanged(nameof(TrainingDetail));
        }
    }
    public PTAcceptSchedule(Guid trainingId, Func<Task> refresh)
	{
		InitializeComponent();
        Setup();
        _trainingId = trainingId;
        _refresh = refresh;
    }
    public async void Setup()
    {
        await FetchData();
    }

	public async Task FetchData()
	{
        loadingDialog.IsVisible = true;
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token == null)
            {
                throw new Exception("Lỗi xác thực. Vui lòng đăng nhập lại");
            }
            var result = await Fetcher.GetAsync<GetTrainingDetailDto>($"api/trainings/{_trainingId}", token);
            if (result != null)
            {
                TrainingDetail = result;
                TrainingDetail.Slots = TrainingDetail.Slots.OrderBy(s => s.StartTime).ToObservableCollection();
            }
            for (var i = 0; i < TrainingDetail.Slots.Count; i++)
            {
                TrainingDetail.Slots[i].SlotNumber = i + 1;
                if (!string.IsNullOrEmpty(TrainingDetail.Slots[i].Location))
                {
                    TrainingDetail.Slots[i].Color = "#FF2E3191";
                }
                else
                {
                    TrainingDetail.Slots[i].Color = "Gray";
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi","Có lỗi xảy ra. Err : " + ex.Message,"OK");
        }
        loadingDialog.IsVisible = false;
        BindingContext = this;
    }

    private async void btnEdit_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            var boundItem = button.CommandParameter as GetSlotDto;
            if (boundItem != null)
            {
                await Navigation.PushModalAsync(new EditWorkoutSessionDetailPage(boundItem.Id, FetchData, boundItem.SlotNumber));
            }
        }
    }

    private async void btnConfirm_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Xác nhận", "Bạn đã kiểm tra kĩ càng và đồng ý duyệt lịch cho người này?", "Có", "Không");
        if (answer)
        {
            loadingDialog.IsVisible = true;
            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (token == null)
                {
                    throw new Exception("Lỗi xác thực. Vui lòng đăng nhập lại");
                }
                if (TrainingDetail.PricePerSlot == null || TrainingDetail.PricePerSlot.Value < 150000 || TrainingDetail.PricePerSlot.Value > 1000000)
                {
                    throw new Exception("Giá tiền mỗi buổi của bạn vượt giới hạn (150k - 1 triệu)");
                }
                foreach (var slot in TrainingDetail.Slots)
                {
                    if (string.IsNullOrEmpty(slot.Location))
                    {
                        throw new Exception("Một số buổi tập chưa được cập nhật địa điểm");
                    }
                }
                await Fetcher.PatchAsync($"api/trainings/approving", new RequestApproveTrainingDto
                {
                    TrainingId = _trainingId,
                    MinuteDistance = 5
                } , token);
                await Fetcher.PatchAsync($"api/trainings/update-price", new RequestUpdateTrainingPriceDto
                {
                    TrainingId= _trainingId,
                    TrainingPrice = TrainingDetail.PricePerSlot.Value
                }, token);
                await DisplayAlert("Thành công", "Đã duyệt lịch cho người này", "OK");
                while (Shell.Current.Navigation.ModalStack.Count > 0)
                {
                    await Shell.Current.Navigation.PopModalAsync(false); // Set false to prevent animation
                }
                await _refresh();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
            }
            loadingDialog.IsVisible = false;
        }
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}