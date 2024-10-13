namespace FitSwipe.Mobile.Pages.ReportPages;

public partial class HandleReport : ContentPage
{
    //private bool _isAccept;
    public HandleReport()
	{
		InitializeComponent();
	}

    //public bool IsAccept
    //{
    //    get => _isAccept;
    //    set
    //    {
    //        _isAccept = value;
    //        OnPropertyChanged();
    //    }
    //}
    private async void Tick_Clicked(object sender, EventArgs e)
    {
        //IsAccept = true;
    }
    private async void Cancel_Clicked(object sender, EventArgs e)
    {
        
    }
    private async void Remove_Clicked(object sender, EventArgs e)
    {
        
    }
    private async void Transfer_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AcceptHandleReport());
    }
}