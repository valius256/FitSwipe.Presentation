using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FitSwipe.Mobile.ViewModels
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        private readonly HttpClient _httpClient;
        [ObservableProperty]
        private string _email = string.Empty;
        [ObservableProperty]
        private string _password = string.Empty;

        public string UserName => _authClient.User?.Info.DisplayName;
        public LoadingDialog LoadingDialog { get; set; } = new LoadingDialog();

        public SignInViewModel(FirebaseAuthClient authClient, IHttpClientFactory httpClientFactory)
        {
            _authClient = authClient;
            _httpClient = httpClientFactory.CreateClient("BackendApiClient");
        }

        [RelayCommand]
        private async Task SignIn()
        {
            LoadingDialog.IsVisible = true;
            try
            {

                var signInDto = new SignInDto
                {
                    Email = _email,
                    Password = _password
                };

                var content = new StringContent(JsonSerializer.Serialize(signInDto), Encoding.UTF8, "application/json");

                // Sign in with Firebase
                var response = await _httpClient.PostAsync("api/Authentication/login-firebase", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthenResponseDto>(responseContent);

                if (authResponse != null)
                {
                    // Handle the response
                    if (authResponse.Code.Equals("OK"))
                    {
                        await SecureStorage.SetAsync("auth_token", authResponse.Message);
                        
                        var token = await SecureStorage.GetAsync("auth_token");

                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var testResponse = await _httpClient.GetAsync("api/Authentication/get-test");
                        var result = await testResponse.Content.ReadAsStringAsync();
                        // Successful login logic
                        await Application.Current.MainPage.DisplayAlert("Success", $"{result}", "OK");
                        // Optionally, navigate to another page or store token

                        await Shell.Current.GoToAsync("//PTList");
                    }
                    else
                    {
                        // Handle errors based on the message
                        await Application.Current.MainPage.DisplayAlert("Error", authResponse.Message, "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Unexpected response format.", "OK");
                }

            }
            catch (Exception ex)
            {
                LoadingDialog.IsVisible = false;
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
