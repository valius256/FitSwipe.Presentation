using CommunityToolkit.Maui.Core.Views;
using FitSwipe.Shared.Dtos.Others;
using Microsoft.Maui.Handlers;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class BookSlotModal : ContentView
{
    private DateTime _start = DateTime.Now;
    private DateTime _end = DateTime.Now;
    private TimeSpan _timeBegin = TimeSpan.Zero;
    private TimeSpan _timeEnd = TimeSpan.Zero;
    private Guid? _baseSlotId;
    private Guid? _editSlotId;
    public event EventHandler<BookSlotEventArgs>? OnBookSlot;
    public event EventHandler<BookSlotEventArgs>? OnEditSlot;
    public event EventHandler<BookSlotEventArgs>? OnDeleteSlot;
    public BookSlotModal()
	{
		InitializeComponent();
        Hide();
    }
    public void SetTime(DateTime start, DateTime end, Guid? id, Guid? editId = null, DateTime? editSlotStart = null, DateTime? editSlotEnd = null)
    {
        _start = start;
        _end = end;
        _baseSlotId = id;
        _editSlotId = editId;
        if (editId != null)
        {
            btnDelete.IsVisible = true;
            if (editSlotEnd != null && editSlotStart != null)
            {
                tpBegin.Time = editSlotStart.Value.TimeOfDay;
                tpEnd.Time = editSlotEnd.Value.TimeOfDay;
            }
        } else
        {
            btnDelete.IsVisible = false;
        }
        if (_baseSlotId == null)
        {
            txtTimeConstraint.IsVisible = false;
        } else
        {
            txtTimeConstraint.IsVisible = true;
        }
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

    private void btnApprove_Clicked(object sender, EventArgs e)
    {
        if (_editSlotId != null)
        {
            OnEditSlot?.Invoke(this, new BookSlotEventArgs(_start, _end, _timeBegin, _timeEnd, _baseSlotId, _editSlotId));
        }
        else
        {
            OnBookSlot?.Invoke(this, new BookSlotEventArgs(_start,_end,_timeBegin,_timeEnd,_baseSlotId));
        }
    }


    private void btnDelete_Clicked(object sender, EventArgs e)
    {
        OnDeleteSlot?.Invoke(this, new BookSlotEventArgs(_start, _end, _timeBegin, _timeEnd, _baseSlotId, _editSlotId));
    }
    public class BookSlotEventArgs : EventArgs
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan TimeBegin { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public Guid? BaseSlotId { get; set; }
        public Guid? EditSlotId { get; set; }
        public BookSlotEventArgs(DateTime start, DateTime end, TimeSpan timeBegin, TimeSpan timeEnd, Guid? baseSlotId, Guid? editSlotId = null)
        {
            Start = start;
            End = end;
            TimeBegin = timeBegin;
            TimeEnd = timeEnd;
            BaseSlotId = baseSlotId;
            EditSlotId = editSlotId;
        }
    }
}

