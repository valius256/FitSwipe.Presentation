using FitSwipe.Mobile.Pages.SetupPages;
using FitSwipe.Mobile.Utils;
using FitSwipe.Shared.Dtos.Users;
using Microsoft.Maui.Storage;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FitSwipe.Mobile.Pages;

public partial class CertificateUploadView : ContentPage, INotifyPropertyChanged
{
    private bool _isPhotoCaptured;
    private ImageSource _capturedImageSource;
    private UpdateUserProfileDto _currentUser;
    private FileResult? _fileResult;
    public List<Guid> AlreadyTags;

    public CertificateUploadView (UpdateUserProfileDto updateUserProfileDto, List<Guid> tags)
    {
        InitializeComponent();
        _currentUser = updateUserProfileDto;
        AlreadyTags = tags;
        BindingContext = this;
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

    private async void cameraPhotoButton_Clicked (object sender, EventArgs e)
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    _fileResult = photo;
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

    private async void galleryPhotoButton_Clicked (object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.Default.PickPhotoAsync();
            if (result != null)
            {
                _fileResult = result;
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

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void btnUpload_Clicked(object sender, EventArgs e)
    {
        if (_fileResult != null)
        {
            loadingDialog.IsVisible = true;
            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                var url = await MauiUtils.UploadImageAsync(_fileResult, token ?? string.Empty);
                if (url == null)
                {
                    throw new Exception("Lỗi đăng ảnh");
                }
                await Navigation.PushModalAsync(new SetupProfileStepFinalPage(_currentUser, AlreadyTags, url.FileUrl));
            }
            catch (Exception)
            {
                await DisplayAlert("Lỗi", "Đăng ảnh thất bại, vui lòng thừ lại sau", "OK");
            }
            loadingDialog.IsVisible = false;
        }
    }

    private async void btnSkip_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SetupProfileStepFinalPage(_currentUser, AlreadyTags));
    }

    //// Notify property changed for data binding
    //public event PropertyChangedEventHandler PropertyChanged;
    //protected void OnPropertyChanged ([CallerMemberName] string propertyName = null)
    //{
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //}

}