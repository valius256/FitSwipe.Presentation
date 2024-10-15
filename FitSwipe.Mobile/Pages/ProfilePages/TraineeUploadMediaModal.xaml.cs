using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FitSwipe.Mobile.Pages.ProfilePages;

public partial class TraineeUploadMediaModal : ContentPage
{
    private bool _isPhotoCaptured = false;
    private int _activeTab = 0;
    private string? _capturedImageSource;

    // Declare ImageItems as a class-level property
    private ObservableCollection<string> _imageItems { get; set; } = [];

    public TraineeUploadMediaModal()
    {
        InitializeComponent();
        HandleSwitchTab();
        BindingContext = this;
    }
    public int ActiveTab
    {
        get => _activeTab;
        set
        {
            _activeTab = value;
            OnPropertyChanged();
        }
    }
    public bool IsPhotoCaptured
    {
        get => _isPhotoCaptured;
        set
        {
            _isPhotoCaptured = value;
            OnPropertyChanged();
        }
    }
    public string? CapturedImageSource
    {
        get => _capturedImageSource;
        set
        {
            _capturedImageSource = value;
            OnPropertyChanged(nameof(CapturedImageSource));
        }
    }
    public ObservableCollection<string> ImageItems
    {
        get => _imageItems;
        set
        {
            _imageItems = value;
            OnPropertyChanged(nameof(ImageItems));
        }
    }
    private async void cameraPhotoButton_Clicked(object sender, EventArgs e)
    {

        try
        {
            var result = await MediaPicker.CapturePhotoAsync();
            if (result != null)
            {
                //var stream = await result.OpenReadAsync();
                //var imageSource = ImageSource.FromStream(() => stream);
                if (ActiveTab == 0)
                {

                    CapturedImageSource = result.FullPath;
                    // Show thing a invisable 2
                    IsPhotoCaptured = true;
                } else
                {
                    ImageItems.Add(result.FullPath);
                }

            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., display an alert)
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void galleryPhotoButton_Clicked(object sender, EventArgs e)
    {

        try
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (ActiveTab == 0)
            {

                CapturedImageSource = result.FullPath;
                // Show thing a invisable 2
                IsPhotoCaptured = true;
            }
            else
            {
                ImageItems.Add(result.FullPath);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., display an alert)
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void NextPageUpload_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new UserUploadImageVideo());
        ActiveTab = 1;
        HandleSwitchTab();

    }
    private void Undo_Clicked(object sender, EventArgs e)
    {
        // Show thing a invisable 1
        IsPhotoCaptured = false;
    }

    private void btnTabAvatar_Clicked(object sender, EventArgs e)
    {
        ActiveTab = 0;
        HandleSwitchTab();

    }

    private void HandleSwitchTab()
    {
        switch (ActiveTab)
        {
            case 0:
                tab1Content.IsVisible = true;
                tab2Content.IsVisible = false;
                btnTabMedia.BackgroundColor = Colors.White;
                btnTabAvatar.BackgroundColor = Color.FromArgb("#FF52BB00");
                btnTabAvatar.TextColor = Colors.White;
                btnTabMedia.TextColor = Colors.Black;
                btnTabAvatar.ZIndex = 1;
                btnTabMedia.ZIndex = 0;
                break;
            case 1:
                btnTabAvatar.BackgroundColor = Colors.White;
                btnTabMedia.BackgroundColor = Color.FromArgb("#FF52BB00");
                btnTabAvatar.TextColor = Colors.Black;
                btnTabMedia.TextColor = Colors.White;
                tab1Content.IsVisible = false;
                tab2Content.IsVisible = true;
                btnTabAvatar.ZIndex = 0;
                btnTabMedia.ZIndex = 1;
                break;
        }
    }

}


