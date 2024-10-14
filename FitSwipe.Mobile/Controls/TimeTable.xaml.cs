using FitSwipe.Mobile.Extensions;
using FitSwipe.Mobile.Pages.SchedulePages;
using FitSwipe.Mobile.Pages.TrainingPages;
using FitSwipe.Shared.Dtos.Others;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using MauiIcons.Core;
using MauiIcons.Fluent;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FitSwipe.Mobile.Controls;

public partial class TimeTable : ContentView
{
	public const int minHour = 4;
	public const int maxHour = 22;
    public ObservableCollection<GetSlotDto> Slots = new ObservableCollection<GetSlotDto>();
	public List<string> TimeStampDisplays {  get; set; } = new List<string>();
    public GetWeekDto CurrentWeek { get; set; }
	//public List<GetWeekDto> Weeks {  get; set; } = new List<GetWeekDto>();
    public int ZoomLevel { get; set; } = -1;

    public static readonly BindableProperty ThemeProperty =
            BindableProperty.Create(nameof(Theme), typeof(string), typeof(TimeTable), "#52BB00");

    public static readonly BindableProperty YearProperty =
            BindableProperty.Create(nameof(Year), typeof(int), typeof(TimeTable), DateTime.Now.Year);

    public event EventHandler? WeekChanged;
    public event EventHandler<SlotEventArgs>? SlotAction;

    protected virtual void OnWeekChanged(EventArgs e)
    {
        // If there are any subscribers, raise the event
        WeekChanged?.Invoke(this, e);
    }
    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public int Year
    {
        get => (int)GetValue(YearProperty);
        set => SetValue(YearProperty, value);
    }
    public TimeTable()
	{
		InitializeComponent();
		ZoomIn(true);

        var currentYear = DateTime.Now.Year;
        yearPicker.ItemsSource = new List<int> { currentYear, currentYear - 1, currentYear - 2, currentYear - 3};
        yearPicker.SelectedIndex = 0;
        CurrentWeek = new GetWeekDto()
        {
            StartDate = DateOnly.FromDateTime(Helper.GetFirstDayOfWeek()),
            EndDate = DateOnly.FromDateTime(Helper.GetLastDayOfWeek())
        };
        _ = new MauiIcon();
		BindingContext = this;
    }
	public void ZoomIn(bool positive)
	{
		if ((positive && ZoomLevel < 2) || (!positive && ZoomLevel > -2))
		{
            ZoomLevel += positive ? 1 : -1;
			int distanceInMinutes = (ZoomLevel) switch
			{
				-2 => 240,
				-1 => 120,
				0 => 60,
				1 => 30,
				2 => 15,
				_ => 60
			};
			int totalStamps = (maxHour - minHour) * 60 / distanceInMinutes;
			int hour = minHour;
			int minute = 0;
            TimeStampDisplays = new List<string>();
            for (int i = 0; i <= totalStamps; i++)
			{
                string timeString = hour.ToString() + ":" + minute.ToString().PadLeft(2,'0');
                TimeStampDisplays.Add(timeString);
				minute += distanceInMinutes;
				double minDivided = minute / 60;
				if (minDivided >= 1)
				{
					hour += (int)Math.Floor(minDivided);
					minute = 0;
                }
            }
			GenerateLabel();
            RenderSlot();
        }
	}
	public void SetSlots(ObservableCollection<GetSlotDto> slots)
	{
		Slots = slots;
        RenderSlot();
    }
    public void GenerateLabel()
	{
		timeStamps.Clear();
		foreach (string timeString in TimeStampDisplays)
		{
			timeStamps.Children.Add(new Label
			{
				Text = timeString,
				TextColor = Color.FromArgb("#FF000000"),
				FontAttributes = FontAttributes.Bold,
				HeightRequest = 40,
				Padding= new Thickness(4,0,4,0),
			});
		}
	}
	public void RenderSlot()
	{
		var monSlots = Slots.Where(s => s.StartTime.DayOfWeek == DayOfWeek.Monday && IsContainedInCurrentWeek(s.StartTime)).ToList(); 
		var tueSlots = Slots.Where(s => s.StartTime.DayOfWeek == DayOfWeek.Tuesday && IsContainedInCurrentWeek(s.StartTime)).ToList(); 
		var wedSlots = Slots.Where(s => s.StartTime.DayOfWeek == DayOfWeek.Wednesday && IsContainedInCurrentWeek(s.StartTime)).ToList(); 
		var thuSlots = Slots.Where(s => s.StartTime.DayOfWeek == DayOfWeek.Thursday && IsContainedInCurrentWeek(s.StartTime)).ToList(); 
		var friSlots = Slots.Where(s => s.StartTime.DayOfWeek == DayOfWeek.Friday && IsContainedInCurrentWeek(s.StartTime)).ToList(); 
		var satSlots = Slots.Where(s => s.StartTime.DayOfWeek == DayOfWeek.Saturday && IsContainedInCurrentWeek(s.StartTime)).ToList();
		var sunSlots = Slots.Where(s => s.StartTime.DayOfWeek == DayOfWeek.Sunday && IsContainedInCurrentWeek(s.StartTime)).ToList();

		RenderSlotForADayInAWeek(mondayColumn, monSlots);
		RenderSlotForADayInAWeek(tuesdayColumn, tueSlots);
		RenderSlotForADayInAWeek(wednesdayColumn, wedSlots);
		RenderSlotForADayInAWeek(thursdayColumn, thuSlots);
		RenderSlotForADayInAWeek(fridayColumn, friSlots);
		RenderSlotForADayInAWeek(saturdayColumn, satSlots);
		RenderSlotForADayInAWeek(sundayColumn, sunSlots);
    }
    public void RenderSlotForADayInAWeek(AbsoluteLayout dayColumn, List<GetSlotDto> slots)
    {
        dayColumn.Clear();
		dayColumn.Add(new BoxView
		{
			Color = Color.FromArgb("#FF000000")
		}, new Rect(0, 1, 1, 1), AbsoluteLayoutFlags.WidthProportional);
        int distanceInMinutes = (ZoomLevel) switch
        {
            -2 => 240,
            -1 => 120,
            0 => 60,
            1 => 30,
            2 => 15,
            _ => 60
        };
        foreach (var slot in slots)
        {
            var slotGrid = new Grid();

            var slotDuration = slot.EndTime - slot.StartTime;

            var y = 5 + (new TimeSpan(slot.StartTime.Hour, slot.StartTime.Minute, slot.StartTime.Second) - new TimeSpan(minHour, 0, 0)).TotalHours * 40 * 60 / distanceInMinutes;
			var h = slotDuration.TotalHours * 40 * 60 / distanceInMinutes;

            var borderShape = new RoundRectangle();
            borderShape.CornerRadius = new CornerRadius(5);
            var border = new Border
            {
                BackgroundColor = Color.FromArgb(slot.Color),
                StrokeShape = borderShape,
            };
            if (h > 50)
			{
                var content = new VerticalStackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                content.Children.Add(new Label
                {
                    Text = GetSimpleTime(slot.StartTime),
                    TextColor = Color.FromArgb("#FFFFFFFF"),
                    FontAttributes = FontAttributes.Bold,
					FontSize = 9
                });
                content.Children.Add(new BoxView
                {
                    HeightRequest = 1,
                    Color = Color.FromArgb("#FFFFFFFF"),
                    Margin = new Thickness(2,3)
                });
                content.Children.Add(new Label
                {
                    Text = GetSimpleTime(slot.EndTime),
                    TextColor = Color.FromArgb("#FFFFFFFF"),
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 9
                });
                border.Content = content;
            }
            slotGrid.Add(border);
            if (slot.PaymentStatus != PaymentStatus.Paid && slot.Status != SlotStatus.Unbooked && slot.Status != SlotStatus.Pending && slot.Status != SlotStatus.Disabled)
            {
                slotGrid.Add(new Image
                {
                    WidthRequest = 20,
                    HeightRequest = 20,
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Start,
                    Source = slot.PaymentStatus == PaymentStatus.NotPaid ? "white_warning" : "red_warning"
                });
            }
            dayColumn.Add(slotGrid, new Rect(0, y, 1, h), AbsoluteLayoutFlags.WidthProportional);
			AddActionForSlot(border, slot);
        }
    }
	private void AddActionForSlot(Border border, GetSlotDto slot)
	{
        SlotAction?.Invoke(this, new SlotEventArgs(border, slot));
    }
    public void GotoWeek(DateOnly date)
    {
        var weeks = ((List<GetWeekDto>)weekPicker.ItemsSource);
        foreach (var week in weeks)
        {
            if (week.StartDate <= date && week.EndDate >= date)
            {
                weekPicker.SelectedItem = week;
                return;
            }
        }
    }
    private bool IsContainedInCurrentWeek(DateTime dateTime)
    {
        return dateTime >= CurrentWeek.StartDate.ToDateTime(TimeOnly.MinValue) && dateTime <= CurrentWeek.EndDate.ToDateTime(TimeOnly.MaxValue);
    }
	private string GetSimpleTime(DateTime time)
	{
		return time.Hour.ToString() + ":" + time.Minute.ToString().PadLeft(2, '0');
	}
    private void btnZoomIn_Clicked(object sender, EventArgs e)
    {
		ZoomIn(true);
    }

    private void btnZoomOut_Clicked(object sender, EventArgs e)
    {
        ZoomIn(false);
    }
 
    private void ModeOneAction(object sender, EventArgs e)
	{

	}

    private void btnPrevWeek_Clicked(object sender, EventArgs e)
    {
        if (weekPicker.SelectedIndex > 0)
        {
            // Move to the previous week
            weekPicker.SelectedIndex -= 1;
        }
    }

    private void btnNextWeek_Clicked(object sender, EventArgs e)
    {
        if (weekPicker.SelectedIndex < weekPicker.ItemsSource.Count - 1)
        {
            // Move to the next week
            weekPicker.SelectedIndex += 1;
        }
    }

    private void weekPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        CurrentWeek = ((List<GetWeekDto>)weekPicker.ItemsSource)[weekPicker.SelectedIndex];
        WeekChanged?.Invoke(this, e);
    }

    private void yearPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Year = (int)yearPicker.SelectedItem;
        var weeks = Helper.GetWeeksOfYear(Year);
        weekPicker.ItemsSource = weeks;
        if (Year == DateTime.Now.Year)
        {
            var currentWeek = Helper.GetCurrentWeek(weeks);
            // Set the default selected item to the current week
            if (currentWeek != null)
            {
                CurrentWeek = currentWeek;
                weekPicker.SelectedItem = currentWeek;
            }
        } else
        {
            weekPicker.SelectedIndex = 0;
        }
    }
}

