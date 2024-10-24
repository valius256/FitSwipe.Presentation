using FitSwipe.Shared.Utils;

namespace FitSwipe.Mobile
{
    public partial class AppShell : Shell
    {
        const string FirstLaunchKey = "IsFirstLaunch";

        public AppShell()
        {
            InitializeComponent();

            
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Check if this is the first time the app is launched
            if (IsFirstTimeUser())
            {
                // Navigate to WelcomePage for first-time users
                await Shell.Current.GoToAsync("//Started");
                Preferences.Set(FirstLaunchKey, false); // Update the flag
            }
            else
            {
                await Navigate();
            }
        }
        private async Task Navigate()
        {
            var role = await SecureStorage.GetAsync("loginedRole");
            if (role != null)
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (token != null)
                {
                    var user = await Shortcut.GetLoginedUser(token);
                    if (user != null)
                    {
                        if (user.City == null || user.Job == null || (DateTime.Now.Year - user.DateOfBirth.Year < 5))
                        {
                            await Current.GoToAsync("//SetupProfile");
                            return;
                        }
                        if (role == "PT")
                        {
                            await Current.GoToAsync("//PTHomePage");
                            return;
                        }
                        else
                        {
                            await Current.GoToAsync("//PTList");
                            return;
                        }
                    }
                    
                }
            }  
            await Current.GoToAsync("//SignIn");            
        }
        private bool IsFirstTimeUser()
        {
            // Default value is true if the key is not found
            return Preferences.Get(FirstLaunchKey, true);
        }
    }
}
