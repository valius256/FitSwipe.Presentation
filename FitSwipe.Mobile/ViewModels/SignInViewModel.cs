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

                // Adjust UI or store the role based on your application's needs
                OnPropertyChanged(nameof(UserName));
                await Application.Current.MainPage.DisplayAlert("Success", $"You {Email} have successfully logged in with role", "OK");


                // Retrieve the Firebase ID token from the result
                var idToken = await result.User.GetIdTokenAsync(false);

                // Send the token to the backend API to verify and get the user's role
                //    var httpClient = new HttpClient();
                //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

                //    // Use the IP address of your development machine instead of localhost
                //    var response = await httpClient.GetAsync("http://192.168.2.53:5250/api/Authentication/verify-token");

                //    if (response.IsSuccessStatusCode)
                //    {
                //        var responseContent = await response.Content.ReadAsStringAsync();
                //        var jsonResponse = JsonSerializer.Deserialize<ResponseRoleModel>(responseContent);

                //        // Assume ResponseRoleModel has a Role property
                //        var userRole = jsonResponse?.Role;

                //        // Adjust UI or store the role based on your application's needs
                //        OnPropertyChanged(nameof(UserName));
                //        await Application.Current.MainPage.DisplayAlert("Success", $"You {Email} have successfully logged in with role {userRole}", "OK");
                //    }
                //    else
                //    {
                //        await Application.Current.MainPage.DisplayAlert("Error", $"Server error: {response.StatusCode}", "OK");
                //    }
                //}
                //catch (HttpRequestException httpEx)
                //{
                //    await Application.Current.MainPage.DisplayAlert("Network Error", $"Network error: {httpEx.Message}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }


        [RelayCommand]
        private async Task NavigateSignUp()
        {
            await Shell.Current.GoToAsync("//SignUp");
        }
    }
}
