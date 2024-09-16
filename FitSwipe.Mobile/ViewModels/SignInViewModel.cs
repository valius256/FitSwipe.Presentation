using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Dtos;
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

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

                // Send POST request to backend API
                // tam thoi m lay dia chi may m chay backend lam cai api xuat la dc
                // gui lay role duoc roi chi co cho authen no dang loi 1 chut

                var response = await _httpClient.PostAsync("api/Authentication/verify-token", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonSerializer.Deserialize<ResponseRoleModel>(responseContent);

                    var userRole = jsonResponse?.Role;

                    // Adjust UI or store the role based on your application's needs
                    OnPropertyChanged(nameof(UserName));

                    //await Application.Current.MainPage.DisplayAlert("Success", $"You {Email} have successfully logged in with role {userRole}", "OK");
                    await Shell.Current.GoToAsync("//SetupProfile");
                    LoadingDialog.IsVisible = false;
                }
                else
                {
                    LoadingDialog.IsVisible = false;
                    await Application.Current.MainPage.DisplayAlert("Error", $"Server error: {response.StatusCode}", "OK");
                }
            }
            catch (HttpRequestException httpEx)
            {
                LoadingDialog.IsVisible = false;
                await Application.Current.MainPage.DisplayAlert("Network Error", $"Network error: {httpEx.Message}", "OK");
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
