namespace FitSwipe.Mobile.Controls;

public partial class LoadingDialog : ContentView
{
    public static readonly BindableProperty MessageProperty =
            BindableProperty.Create(nameof(Message), typeof(string), typeof(LoadingDialog), "Loading.....");

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public LoadingDialog()
	{
		InitializeComponent();
        BindingContext = this;
	}
}