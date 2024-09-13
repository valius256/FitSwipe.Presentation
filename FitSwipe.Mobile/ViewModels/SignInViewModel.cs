using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace FitSwipe.Mobile.ViewModels
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _password;

        public string UserName => _authClient.User?.Info.DisplayName;

        public SignInViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
        }

        [RelayCommand]
        private async Task SignIn()
        {
            try
            {
                var result = await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);
                OnPropertyChanged(nameof(UserName));
                await Application.Current.MainPage.DisplayAlert("Thành Công", $"Bạn {Email} đã đăng nhập thành công ", "OK");

            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Lỗi", "Wrong login password or email", "OK");
            }
        }

        [RelayCommand]
        private async Task NavigateSignUp()
        {
            await Shell.Current.GoToAsync("//SignUp");
        }
    }
}
