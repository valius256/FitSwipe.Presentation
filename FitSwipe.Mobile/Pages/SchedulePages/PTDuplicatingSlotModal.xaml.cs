using FitSwipe.Mobile.Extensions;
using FitSwipe.Shared.Dtos.Others;
using FitSwipe.Shared.Utils;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class PTDuplicatingSlotModal : ContentView
{
	public List<GetWeekDto> Weeks { get; set; } = new List<GetWeekDto>();
	public List<GetWeekDto> SelectedWeeks { get; set; } = new List<GetWeekDto>();
    public static readonly BindableProperty YearProperty =
            BindableProperty.Create(nameof(Year), typeof(int), typeof(PTDuplicatingSlotModal), DateTime.Now.Year);

    public int Year
    {
        get => (int)GetValue(YearProperty);
        set => SetValue(YearProperty, value);
    }
    public event EventHandler<WeekCheckedEventArgs>? OnConfirmed;

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
        OnConfirmed?.Invoke(this,new WeekCheckedEventArgs(SelectedWeeks));
    }

    private void weekCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // Get the CheckBox that triggered the event
        var checkbox = (CheckBox)sender;

        // Get the Week object from the BindingContext
        var week = (GetWeekDto)checkbox.BindingContext;
        if (week != null)
        {
            if (e.Value)
            {
                SelectedWeeks.Add(week);
            }
            else
            {
                SelectedWeeks.Remove(week);
            }
        }
       
    }

    
}