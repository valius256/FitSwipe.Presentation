using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FitSwipe.Mobile.Pages.UploadAvatar;

public partial class UserUploadAvatar : ContentPage, INotifyPropertyChanged
{
    private bool _isPhotoCaptured;
    private ImageSource _capturedImageSource;

    public UserUploadAvatar()
	{
		
        BindingContext = this;
        InitializeComponent();
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
    public ImageSource CapturedImageSource
    {
        get => _capturedImageSource;
        set
        {
            _capturedImageSource = value;
            OnPropertyChanged();
        }
    }
    private async void cameraPhotoButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    // Load the captured image into the Image control
                    var stream = await photo.OpenReadAsync();
                    CapturedImageSource = ImageSource.FromStream(() => stream);

                    // Hide the buttons and display the photo
                    IsPhotoCaptured = true;
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void galleryPhotoButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.Default.PickPhotoAsync();
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                CapturedImageSource = ImageSource.FromStream(() => stream);
                IsPhotoCaptured = true;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // Notify property changed for data binding
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}