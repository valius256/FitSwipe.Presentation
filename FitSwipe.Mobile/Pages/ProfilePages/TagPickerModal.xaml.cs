using FitSwipe.Mobile.Extensions;
using FitSwipe.Shared.Dtos.Others;
using FitSwipe.Shared.Dtos.Tags;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.ProfilePages;

public partial class TagPickerModal : ContentView
{
    private ObservableCollection<GetTagDto> _tags = new ObservableCollection<GetTagDto>();
    public ObservableCollection<GetTagDto> Tags
    {
        get => _tags;
        set
        {
            _tags = value;
            OnPropertyChanged(nameof(Tags));
        }
    }
    public TagPickerModal()
	{
		InitializeComponent();
        Hide();
        BindingContext = this;
    }
    public List<GetTagDto> SelectedTags { get; set; } = new List<GetTagDto>();

    public event EventHandler<TagCheckedEventArgs>? OnConfirmed;

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
    public void SetSelected(List<GetTagDto> tags)
    {
        foreach (var tag in tags)
        {
            var existTag = _tags.FirstOrDefault(t => t.Id == tag.Id);
            if (existTag != null)
            {
                existTag.IsSelected = true;
            }
        }
    }
    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        Hide();
    }
    private void btnAccept_Clicked(object sender, EventArgs e)
    {
        OnConfirmed?.Invoke(this, new TagCheckedEventArgs(SelectedTags));
    }

    private void tagCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // Get the CheckBox that triggered the event
        var checkbox = (CheckBox)sender;

        // Get the Week object from the BindingContext
        var tag = (GetTagDto)checkbox.BindingContext;
        if (tag != null)
        {
            if (e.Value)
            {
                SelectedTags.Add(tag);
            }
            else
            {
                SelectedTags.Remove(tag);
            }
        }
    }
}