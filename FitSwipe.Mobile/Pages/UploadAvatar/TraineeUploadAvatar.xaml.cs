using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FitSwipe.Mobile.Pages.UploadAvatar;

public partial class TraineeUploadAvatar : ContentPage, INotifyPropertyChanged
{
    private bool _isPhotoCaptured;
    private ImageSource _capturedImageSource;

    // Declare ImageItems as a class-level property
    public ObservableCollection<ImageItem> ImageItems { get; set; }

    public TraineeUploadAvatar()
    {
        InitializeComponent();
        BindingContext = this;

        // Initialize the ImageItems collection
        ImageItems = new ObservableCollection<ImageItem>();

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
            OnPropertyChanged(nameof(CapturedImageSource));
        }
    }

    private async void cameraPhotoButton_Clicked(object sender, EventArgs e)
    {

        try
        {
            var result = await MediaPicker.CapturePhotoAsync();
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                var imageSource = ImageSource.FromStream(() => stream);

                
                CapturedImageSource = imageSource;

                // Show thing a invisable 2
                IsPhotoCaptured = true;

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
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                var imageSource = ImageSource.FromStream(() => stream);

                // Add the new image to the collection
                CapturedImageSource = imageSource;

                // Show thing a invisable 2
                IsPhotoCaptured = true;

            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., display an alert)
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // Notify property changed for data binding
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    private async void NextPageUpload_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TraineeUploadImageVideo());
    }
    private async void Undo_Clicked(object sender, EventArgs e)
    {
        // Show thing a invisable 1
        IsPhotoCaptured = false;
    }

    public class ImageItem
    {
        public ImageSource ImageSource { get; set; }
    }
}


