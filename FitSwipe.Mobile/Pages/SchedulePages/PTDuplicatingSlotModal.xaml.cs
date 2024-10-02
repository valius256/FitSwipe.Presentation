using FitSwipe.Shared.Dtos.Others;
using FitSwipe.Shared.Utils;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class PTDuplicatingSlotModal : ContentView
{
	public List<GetWeekDto> Weeks { get; set; } = new List<GetWeekDto>();
    public static readonly BindableProperty YearProperty =
            BindableProperty.Create(nameof(Year), typeof(int), typeof(PTDuplicatingSlotModal), DateTime.Now.Year);

    public int Year
    {
        get => (int)GetValue(YearProperty);
        set => SetValue(YearProperty, value);
    }

    public PTDuplicatingSlotModal()
	{
		InitializeComponent();
		var weeks = Helper.GetWeeksOfYear(Year);
        Weeks = weeks.Where(w => w.StartDate > DateOnly.FromDateTime(DateTime.Now)).ToList();
        Hide();
        BindingContext = this;
    }
    public void Show()
    {
        IsVisible = true;
        InputTransparent = false;
    }

    public void Hide()
    {
        IsVisible = false;
        InputTransparent = true;

    }

    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        Hide();
    }

    private void btnAccept_Clicked(object sender, EventArgs e)
    {
        Hide();
    }
}