using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class EditWorkoutSessionDetailPage : ContentPage
{
  public EditWorkoutSessionDetailViewModel ViewModel;
  public EditWorkoutSessionDetailPage ()
  {
    InitializeComponent();
    ViewModel = new EditWorkoutSessionDetailViewModel();
    BindingContext = ViewModel;
  }
}