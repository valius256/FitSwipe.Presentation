using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace FitSwipe.Mobile.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        public SignUpViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [ObservableProperty] private string _email;
        [ObservableProperty] private string _username;
        [ObservableProperty] private string _password;

        [RelayCommand]
        private async Task SignUp()
        {
            try
            {

                await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password, Username);
                await Application.Current.MainPage.DisplayAlert("Thành Công", $"Bạn {Email} đã đăng ký thành công ", "OK");
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Lỗi", $"", "OK");

            }
        }

        [RelayCommand]
        private async Task NavigateSignIn()
        {
            await Shell.Current.GoToAsync("//SignIn");
        }

    }

}
