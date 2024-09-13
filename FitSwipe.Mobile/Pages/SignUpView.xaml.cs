using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages;

public partial class SignUpView : ContentPage
{
    public SignUpView(SignUpViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}