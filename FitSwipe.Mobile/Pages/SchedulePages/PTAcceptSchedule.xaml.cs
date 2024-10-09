using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class PTAcceptSchedule : ContentPage
{
    private Guid _trainingId;
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
    public PTAcceptSchedule(Guid trainingId)
	{
		InitializeComponent();
        FetchData();
        _trainingId = trainingId;
    }

	public async void FetchData()
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
}