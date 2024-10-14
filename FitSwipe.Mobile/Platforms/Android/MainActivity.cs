using Android.App;
using Android.Content.PM;
using FitSwipe.Mobile.Pages.PayingPages;

namespace FitSwipe.Mobile
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
    Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
    DataScheme = "fitswipe")]
    public class MainActivity : MauiAppCompatActivity
    {
        //protected override async void OnResume()
        //{
        //    base.OnResume();

        //    if (Intent?.Data != null && Intent.Data.Scheme == "fitswipe")
        //    {
        //        var app = App.Current;
        //        if (app != null)
        //        {
        //            var mainPage = app.MainPage;
        //            if (mainPage != null)
        //            {
        //                await mainPage.DisplayAlert("Chào mừng bạn trở về", "Hello World", "OK");
        //                await mainPage.Navigation.PushModalAsync(new PayingView());
        //            }
        //        }
        //    }
        //}
    }
}
