using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Dtos;

namespace FitSwipe.Mobile.Pages;

public partial class SignInView : ContentPage
{
    public List<ViewItem> Items { get; set; } = new List<ViewItem>();
    public SignInView (SignInViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}