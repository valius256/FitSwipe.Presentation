﻿using FitSwipe.Mobile.Pages.SubscriptionPages;

namespace FitSwipe.Mobile.Pages.ProfilePages;

public partial class PTProfileNavbar : ContentView
{
    public static readonly BindableProperty ActiveTabProperty =
            BindableProperty.Create(nameof(ActiveTab), typeof(int), typeof(PTProfileNavbar), 0, propertyChanged: OnTabChanged);

    private string _tab1Color = "#000000";	
	private string _tab2Color = "#000000";	
	private string _tab3Color = "#000000";
    public int ActiveTab
    {
        get => (int)GetValue(ActiveTabProperty);
        set => SetValue(ActiveTabProperty, value);
    }
    public string Tab1Color
	{
		get => _tab1Color;
		set
		{
			_tab1Color = value;
			OnPropertyChanged(nameof(Tab1Color));
		}
	}
    public string Tab2Color
    {
        get => _tab2Color;
        set
        {
            _tab2Color = value;
            OnPropertyChanged(nameof(Tab2Color));
        }
    }
    public string Tab3Color
    {
        get => _tab3Color;
        set
        {
            _tab3Color = value;
            OnPropertyChanged(nameof(Tab3Color));
        }
    }
    public PTProfileNavbar()
	{
		InitializeComponent();
        BindingContext = this;
        
	}
    private void UpdateBackground()
    {
        switch (ActiveTab)
        {
            case 1:
                Tab1Color = "#2E3192";
                Tab2Color = "#000000";
                Tab3Color = "#000000";
                break;
            case 2:
                Tab1Color = "#000000";
                Tab2Color = "#2E3192";
                Tab3Color = "#000000";
                break;
            case 3:
                Tab1Color = "#000000";
                Tab2Color = "#000000";
                Tab3Color = "#2E3192";
                break;
        }
    }
    private static void OnTabChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PTProfileNavbar)bindable;
        control.UpdateBackground();
    }
    private async void tab1_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//PTProfilePage");
    }

    private async void tab2_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new SubscriptionView());
    }

    private async void tab4_Tapped(object sender, TappedEventArgs e)
    {
        if (Application.Current != null && Application.Current.MainPage != null)
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Đăng xuất", "Bạn có chắc chắn muốn đăng xuất không?", "Có", "Không");
            if (answer)
            {
                SecureStorage.Remove("auth_token");
                SecureStorage.Remove("loginedUserId");
                SecureStorage.Remove("loginedRole");
                await Shell.Current.GoToAsync("//SignIn");
            }
        }
    }

    private async void tab3_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//WithdrawRequestPage?isTrainee=false");
    }
}