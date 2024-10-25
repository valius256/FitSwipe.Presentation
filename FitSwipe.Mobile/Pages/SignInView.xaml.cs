using FitSwipe.Mobile.Pages.SetupPages;
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

    private async void btnFacebook_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Tính năng chưa khả dụng", "Tính năng sẽ sớm ra mắt! Xin lỗi vì sự bất tiện này!", "OK");
    }

    private async void btnGoogle_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Tính năng chưa khả dụng", "Tính năng sẽ sớm ra mắt! Xin lỗi vì sự bất tiện này!", "OK");
    }

    private async void tapForgotPassword_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ForgotPassword());
    }
}