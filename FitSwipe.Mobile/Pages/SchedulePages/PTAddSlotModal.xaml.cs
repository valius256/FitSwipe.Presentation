using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Enums;
using Microsoft.Maui.Handlers;

namespace FitSwipe.Mobile.Pages.SchedulePages;

public partial class PTAddSlotModal : ContentView
{
    private TimeSpan _timeBegin = TimeSpan.Zero;
    private TimeSpan _timeEnd = TimeSpan.Zero;
    private DateTime _date = DateTime.Now;

    public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(PTAddSlotModal), string.Empty);

    public static readonly BindableProperty SubtitleProperty =
            BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(PTAddSlotModal), string.Empty);

    public static readonly BindableProperty ModeProperty =
            BindableProperty.Create(nameof(Mode), typeof(SlotModalMode), typeof(PTAddSlotModal), SlotModalMode.Adding);

    public event EventHandler? OnAdded;
    public event EventHandler? OnDeleted;

    public GetSlotDto? RefSlot {  get; set; }

    public SlotModalMode Mode
    {
        get => (SlotModalMode)GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public PTAddSlotModal()
    {
        InitializeComponent();
        Hide();
        BindingContext = this;
    }
    public List<DateTime> GetTimeFrame()
    {
        return new List<DateTime>
        {
            new DateTime(_date.Year,_date.Month, _date.Day,_timeBegin.Hours,_timeBegin.Minutes,0),
            new DateTime(_date.Year,_date.Month, _date.Day,_timeEnd.Hours,_timeEnd.Minutes,0),
        };
    }
    public void Show()
    {
        IsVisible = true;
        InputTransparent = false;
        btnDelete.IsVisible = Mode == SlotModalMode.Editing;
        if (RefSlot != null)
        {
            tpEnd.Time =  RefSlot.EndTime.TimeOfDay;
            tpBegin.Time =  RefSlot.StartTime.TimeOfDay;
            dpDate.Date = RefSlot.StartTime;
            _timeEnd = tpEnd.Time;
            _timeBegin = tpBegin.Time;
            _date = dpDate.Date;

        }
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
        labelCalculating.Text = "(Tổng cộng " + Math.Round((_timeEnd - _timeBegin).TotalHours, 2) + " tiếng)";
        timeBegin.Text = _timeBegin.Hours + ":" + _timeBegin.Minutes.ToString().PadLeft(2, '0');
    }

    private void tpEnd_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var timePicker = (TimePicker)sender;
        _timeEnd = timePicker.Time;
        labelCalculating.Text = "(Tổng cộng " + Math.Round((_timeEnd - _timeBegin).TotalHours, 2) + " tiếng)";
        timeEnd.Text = _timeEnd.Hours + ":" + _timeEnd.Minutes.ToString().PadLeft(2, '0');
    }

    private void dpDate_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var datePicker = (DatePicker)sender;
        _date = datePicker.Date;
        timeDate.Text = _date.Day + "/" + _date.Month + "/" + _date.Year;
    }

    private void tapDate_Tapped(object sender, TappedEventArgs e)
    {
        dpDate.Focus();
#if ANDROID
        var handler = dpDate.Handler as IDatePickerHandler;
        if (handler != null)
        {
            handler.PlatformView.PerformClick();
        }
#endif
    }

    private void btnApprove_Clicked(object sender, EventArgs e)
    {
        OnAdded?.Invoke(this, e);
    }

    private void btnDelete_Clicked(object sender, EventArgs e)
    {
        OnDeleted?.Invoke(this, e);
    }
}