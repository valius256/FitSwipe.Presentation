using CommunityToolkit.Maui.Core.Views;
using Microsoft.Maui.Handlers;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class BookSlotModal : ContentView
{
	public BookSlotModal()
	{
		InitializeComponent();
        Hide();
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
    }

    private void tpEnd_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {

    }
}