using FitSwipe.Mobile.ViewModels;
using System.ComponentModel;

namespace FitSwipe.Mobile.Pages;

public partial class SignUpView : ContentPage
{
    public SignUpView (SignUpViewModel viewModel)
    {
        InitializeComponent();
        viewModel.LoadingDialog = loadingDialog;
        BindingContext = viewModel; // Bind to ViewModel
    }

    private void ptRadioButton_CheckedChanged (object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            ((SignUpViewModel)BindingContext).SelectedRole = "pt";
        }
    }

    private void traineeRadioButton_CheckedChanged (object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            ((SignUpViewModel)BindingContext).SelectedRole = "trainee";
        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            ((SignUpViewModel)BindingContext).IsAgreedWithTos = true;
        } else
        {
            ((SignUpViewModel)BindingContext).IsAgreedWithTos = false;
        }
    }
}
