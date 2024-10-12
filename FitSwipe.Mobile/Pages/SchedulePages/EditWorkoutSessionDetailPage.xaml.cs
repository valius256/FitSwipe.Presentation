using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class EditWorkoutSessionDetailPage : ContentPage
{
    private Func<Task> _refreshData;
    private EditWorkoutSessionDetailViewModel _viewModel { get; set; }
  public EditWorkoutSessionDetailPage (Guid slotId, Func<Task> refreshData)
  {
    InitializeComponent();
    _refreshData = refreshData;
    _viewModel = new EditWorkoutSessionDetailViewModel(slotId, loadingDialog, pageContent);
    BindingContext = _viewModel;
  }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        
        await Navigation.PopModalAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (_viewModel.IsChangedFlag)
        {
            _refreshData();
        }

    }
}