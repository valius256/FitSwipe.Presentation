using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class MyPTListPage : ContentPage
{
    public MyPTListPage()
    {
        InitializeComponent();
        BindingContext = new MyPTListPageViewModel();
    }
}