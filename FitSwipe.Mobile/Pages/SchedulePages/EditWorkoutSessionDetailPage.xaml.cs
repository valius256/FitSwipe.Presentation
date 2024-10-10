using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class EditWorkoutSessionDetailPage : ContentPage
{
  public EditWorkoutSessionDetailPage ()
  {
    InitializeComponent();
    this.BindingContext = new EditWorkoutSessionDetailViewModel();
  }
}