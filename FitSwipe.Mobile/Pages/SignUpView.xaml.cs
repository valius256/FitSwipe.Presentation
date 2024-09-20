using FitSwipe.Mobile.ViewModels;
using System.ComponentModel;

namespace FitSwipe.Mobile.Pages;

public partial class SignUpView : ContentPage
{
    public SignUpView (SignUpViewModel viewModel)
    {
        InitializeComponent();
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
}
