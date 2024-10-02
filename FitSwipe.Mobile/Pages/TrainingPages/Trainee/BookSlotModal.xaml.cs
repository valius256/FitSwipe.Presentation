using CommunityToolkit.Maui.Core.Views;
using Microsoft.Maui.Handlers;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class BookSlotModal : ContentView
{
    private DateTime _start = DateTime.Now;
    private DateTime _end = DateTime.Now;
    private TimeSpan _timeBegin = TimeSpan.Zero;
    private TimeSpan _timeEnd = TimeSpan.Zero;
    public BookSlotModal()
	{
		InitializeComponent();
        Hide();
    }
    public void SetTime(DateTime start, DateTime end)
    {
        _start = start;
        _end = end;
        RedisplayTime();
    }

    private void RedisplayTime()
    {
        string displayStartHour = _start.Hour == 12 ? "12" : (_start.Hour % 12).ToString();
        string displayEndHour = _end.Hour == 12 ? "12" : (_end.Hour % 12).ToString();
        labelStartTime.Text = displayStartHour + ":" + _start.Minute.ToString().PadLeft(2,'0') + " " + (_start.Hour >= 12 ? "P.M" : "A.M");
        labelEndTime.Text = displayEndHour + ":" + _end.Minute.ToString().PadLeft(2,'0') + " " + (_end.Hour >= 12 ? "P.M" : "A.M");
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

    private void btnClose_Clicked(object sender, EventArgs e)
    {
        Hide();
    }

    private void tapBegin_Tapped(object sender, TappedEventArgs e)
    {
        tpBegin.Focus();
#if ANDROID
        var handler = tpBegin.Handler as ITimePickerHandler;
        if (handler != null)
        {
            handler.PlatformView.PerformClick();
        }
#endif
    }

    private void tapEnd_Tapped(object sender, TappedEventArgs e)
    {
        tpEnd.Focus();
#if ANDROID
        var handler = tpEnd.Handler as ITimePickerHandler;
        if (handler != null)
        {
            handler.PlatformView.PerformClick();
        }
#endif

    }

    private void tpBegin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var timePicker = (TimePicker)sender;
        _timeBegin = timePicker.Time;
        labelCalculating.Text = "(Tổng cộng " + Math.Round((_timeEnd - _timeBegin).TotalHours,2) + " tiếng)";
        timeBegin.Text = _timeBegin.Hours + ":" + _timeBegin.Minutes.ToString().PadLeft(2,'0');
    }

    private void tpEnd_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var timePicker = (TimePicker)sender;
        _timeEnd = timePicker.Time;
        labelCalculating.Text = "(Tổng cộng " + Math.Round((_timeEnd - _timeBegin).TotalHours, 2) + " tiếng)";
        timeEnd.Text = _timeEnd.Hours + ":" + _timeEnd.Minutes.ToString().PadLeft(2, '0');
    }
}