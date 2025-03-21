﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Auth;
using FitSwipe.Shared.Utils;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FitSwipe.Mobile.ViewModels
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        // private readonly HttpClient _httpClient;
        [ObservableProperty]
        private string _email = string.Empty;
        [ObservableProperty]
        private string _password = string.Empty;
        [ObservableProperty]
        private bool rememberMe = true;
        public string UserName => _authClient.User?.Info.DisplayName;
        public LoadingDialog LoadingDialog { get; set; } = new LoadingDialog();

        public SignInViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
            //_httpClient = httpClientFactory.CreateClient("BackendApiClient");
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

                //var content = new StringContent(JsonSerializer.Serialize(signInDto), Encoding.UTF8, "application/json");
                var authResponse = await Fetcher.PostAsync<SignInDto, AuthenResponseDto>("api/Authentication/login-firebase", signInDto);
                if (authResponse != null)
                {
                    // Handle the response
                    if (authResponse.Code.Equals("OK"))
                    {
                        await SecureStorage.SetAsync("auth_token", authResponse.Message);
                        var token = await SecureStorage.GetAsync("auth_token");
                        var user = await Shortcut.GetLoginedUser(authResponse.Message);
                        if (user != null)
                        {
                            await SecureStorage.SetAsync("loginedUserId", user.FireBaseId);
                            if (RememberMe)
                            {
                                await SecureStorage.SetAsync("loginedRole", user.Role.ToString());
                            }
                            string barColor, navigateTo = "//SetupProfile";
                            //Routing between trainee and PT
                            barColor = (user.Role == Shared.Enums.Role.Trainee) ? "#52BB00" : "#2E3192";
                            CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb(barColor));
                            CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.LightContent);
                            if (user.City == null || user.Job == null || (DateTime.Now.Year - user.DateOfBirth.Year < 5))
                            {
                                navigateTo = "//SetupProfile";
                            }
                            else if (user.Role == Shared.Enums.Role.PT)
                            {
                                navigateTo = "//PTHomePage";
                            }
                            else if (user.Role == Shared.Enums.Role.Trainee)
                            {
                                navigateTo = "//MyPTList";
                            }
                            await Shell.Current.GoToAsync(navigateTo);
                        } else
                        {
                            await Application.Current.MainPage.DisplayAlert("Lỗi", $"Có sự cố xảy ra, vui lòng đăng nhập lại", "OK");
                        }
                    }
                    else
                    {
                        // Handle errors based on the message
                        await Application.Current.MainPage.DisplayAlert("Lỗi", "Sai tài khoản hoặc mật khẩu", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Unexpected response format.", "OK");
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            LoadingDialog.IsVisible = false;
        }





        [RelayCommand]
        private async Task NavigateSignUp()
        {
            await Shell.Current.GoToAsync("//SignUp");
        }
    }
}
