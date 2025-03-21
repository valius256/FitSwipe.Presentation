﻿using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FitSwipe.Mobile.Controls;
using FitSwipe.Mobile.Pages;
using FitSwipe.Mobile.Pages.SchedulePages;
using FitSwipe.Mobile.Services;
using FitSwipe.Mobile.ViewModels;
using MauiIcons.Fluent;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using System.Net.Http.Headers;

namespace FitSwipe.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCompatibility()
                .UseMauiCommunityToolkitMediaElement()
                .UseFluentMauiIcons()
                .UseMauiCommunityToolkit()
                .UseFFImageLoading() // Initialize FFImageLoading
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //builder.Services.AddHttpClient("api", HttpClient => new Uri("https://localhost:7151/api"));

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyCO7BUe_jNgpY-EL8LM_XW0pzlkfvyFSns",
                AuthDomain = "fit-swipe-161d7.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                },
            }));

            builder.Services.AddHttpClient("BackendApiClient", client =>
                {
                    //client.BaseAddress = new Uri("https://fitandswipeapi.somee.com/");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                });

            builder.Services.AddSingleton<SignInView>();
            builder.Services.AddSingleton<SignUpView>();
            builder.Services.AddSingleton<SignInViewModel>();
            builder.Services.AddSingleton<SignUpViewModel>();
            builder.Services.AddSingleton<SignalRChatService>();

            builder.Services.AddTransient<ChatViewModel>();

            return builder.Build();
        }
    }
}
