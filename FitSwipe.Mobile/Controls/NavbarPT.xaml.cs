namespace FitSwipe.Mobile.Controls;

public partial class NavbarPT : ContentView
{
    public static readonly BindableProperty ActiveTabProperty =
           BindableProperty.Create(nameof(ActiveTab), typeof(int), typeof(NavbarPT), 0, propertyChanged: OnTabChanged);
    public bool HomeFlag { get; set; } = false;
    public bool ChatFlag { get; set; } = false;
    public bool TrainingFlag { get; set; } = false;
    public bool ScheduleFlag { get; set; } = false;
    public bool ProfileFlag { get; set; } = false;
    public NavbarPT()
    {
        InitializeComponent();
        UpdateBackground();
    }
    public int ActiveTab
    {
        get => (int)GetValue(ActiveTabProperty);
        set => SetValue(ActiveTabProperty, value);
    }
    private string _backgroundTab1 = "White";
    public string BackgroundTab1
    {
        get => _backgroundTab1;
        set
        {
            _backgroundTab1 = value;
            OnPropertyChanged(nameof(BackgroundTab1)); // Notify UI of the change
        }
    }

    private string _backgroundTab2 = "White";
    public string BackgroundTab2
    {
        get => _backgroundTab2;
        set
        {
            _backgroundTab2 = value;
            OnPropertyChanged(nameof(BackgroundTab2));
        }
    }

    private string _backgroundTab3 = "White";
    public string BackgroundTab3
    {
        get => _backgroundTab3;
        set
        {
            _backgroundTab3 = value;
            OnPropertyChanged(nameof(BackgroundTab3));
        }
    }

    private string _backgroundTab4 = "White";
    public string BackgroundTab4
    {
        get => _backgroundTab4;
        set
        {
            _backgroundTab4 = value;
            OnPropertyChanged(nameof(BackgroundTab4));
        }
    }

    private string _backgroundTab5 = "White";
    public string BackgroundTab5
    {
        get => _backgroundTab5;
        set
        {
            _backgroundTab5 = value;
            OnPropertyChanged(nameof(BackgroundTab5));
        }
    }
    private void UpdateBackground()
    {
        BackgroundTab1 = (ActiveTab == 1 ? "DarkBlue" : "White");
        BackgroundTab2 = (ActiveTab == 2 ? "DarkBlue" : "White");
        BackgroundTab3 = (ActiveTab == 3 ? "DarkBlue" : "White");
        BackgroundTab4 = (ActiveTab == 4 ? "DarkBlue" : "White");
        BackgroundTab5 = (ActiveTab == 5 ? "DarkBlue" : "White");
    }
    private static void OnTabChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (NavbarPT)bindable;
        control.UpdateBackground();
    }

    private void navHome_Tapped(object sender, TappedEventArgs e)
    {
        Shell.Current.GoToAsync($"//PTHomePage?flag={HomeFlag}");
        HomeFlag = false;
    }

    private void navChat_Tapped(object sender, TappedEventArgs e)
    {
        Shell.Current.GoToAsync($"//ChatPage?flag={ChatFlag}&role=PT");
        ChatFlag = false;


    }

    private void navTraining_Tapped(object sender, TappedEventArgs e)
    {
        Shell.Current.GoToAsync($"//TrainingPage?flag={TrainingFlag}");
        TrainingFlag = false;


    }

    private void navSchedule_Tapped(object sender, TappedEventArgs e)
    {
        Shell.Current.GoToAsync($"//PTSchedulePage?flag={ScheduleFlag}");
        ScheduleFlag = false;


    }

    private void navProfile_Tapped(object sender, TappedEventArgs e)
    {
        Shell.Current.GoToAsync($"//PTProfilePage?flag={ProfileFlag}");
        ProfileFlag = false;
    }

}