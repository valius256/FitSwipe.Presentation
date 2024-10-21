using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Dtos.Auth;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;

namespace FitSwipe.Mobile.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        public LoadingDialog LoadingDialog { get; set; } = new LoadingDialog();


        public SignUpViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
            selectedRole = "trainee";
        }

        [ObservableProperty] private string _firstName;
        [ObservableProperty] private string _lastName;
        [ObservableProperty] private string _email;
        [ObservableProperty] private string _username;
        [ObservableProperty] private string _phoneNumber;
        [ObservableProperty] private string _password;
        [ObservableProperty] private string _confirmPassword;
        [ObservableProperty] private string selectedRole;
        [ObservableProperty] private bool isAgreedWithTos = false;

        [RelayCommand]
        private async Task SignUp()
        {
            try
            {
                if ((_firstName == null && _lastName == null) || _email == null || _phoneNumber == null || _password == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Thiếu thông tin", $"Vui lòng điền đầy đủ thông tin", "OK");
                }
                else if (_password != _confirmPassword)
                {
                    await Application.Current.MainPage.DisplayAlert("Sai thông tin", $"Mật khẩu không khớp", "OK");
                } else if (!isAgreedWithTos)
                {
                    await Application.Current.MainPage.DisplayAlert("Điều khoản dịch vụ", $"Hãy đồng ý với điều khoản dịch vụ của chúng tôi để đăng ký", "OK");
                }
                else
                {
                    LoadingDialog.IsVisible = true;
                    await Fetcher.PostAsync("api/Authentication/register", new RegisterRequestDto
                    {
                        Email = _email,
                        Password = _password,
                        Name = _firstName + " " + _lastName,
                        Phone = _phoneNumber,
                        Role = selectedRole == "trainee" ? Role.Trainee : Role.PT
                    });
                    await Shell.Current.GoToAsync($"//RegisterSuccessfully?role={selectedRole}");
                }
                
                //await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password, Username);
                //await Application.Current.MainPage.DisplayAlert("Thành Công", $"Bạn {Email} đã đăng ký thành công ", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Lỗi", $"Email này đã được đăng ký", "OK");

            }
            LoadingDialog.IsVisible = false;
        }

        [RelayCommand]
        private async Task NavigateSignIn()
        {
            await Shell.Current.GoToAsync("//SignIn");
        }

    }

}
