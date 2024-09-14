using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using FitSwipe.Shared.Dtos;
using System.Text;
using System.Text.Json;

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

                // Retrieve the Firebase ID token from the result
                var idToken = await result.User.GetIdTokenAsync(false);

                // Create the request body
                var requestBody = new TokenVerificationRequest
                {
                    Token = idToken
                };

                // Serialize the request body
                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                // Set up HttpClient
                var httpClient = new HttpClient();

                // Send POST request to backend API
                var response = await httpClient.PostAsync("http://localhost:5250/api/Authentication/verify-token", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonSerializer.Deserialize<ResponseRoleModel>(responseContent);

                    var userRole = jsonResponse?.Role;

                    // Adjust UI or store the role based on your application's needs
                    OnPropertyChanged(nameof(UserName));
                    await Application.Current.MainPage.DisplayAlert("Success", $"You {Email} have successfully logged in with role {userRole}", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Server error: {response.StatusCode}", "OK");
                }
            }
            catch (HttpRequestException httpEx)
            {
                await Application.Current.MainPage.DisplayAlert("Network Error", $"Network error: {httpEx.Message}", "OK");
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
