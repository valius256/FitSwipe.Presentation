using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Dtos;

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