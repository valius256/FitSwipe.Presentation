using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages;

public partial class SignInView : ContentPage
{

    public SignInView (SignInViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.LoadingDialog = loadingDialog;
    }
}