using FitSwipe.Mobile.Utils;
using FitSwipe.Shared.Dtos.Medias;
using FitSwipe.Shared.Dtos.Upload;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class UserUploadAvatarModal : ContentView
{
	public UserUploadAvatarModal()
	{
		InitializeComponent();
        Hide();
        BindingContext = this;
    }
    private string _theme = "#52BB00";
    private bool _isPhotoCaptured = false;
    private bool _isChangedAvatar = false;
    private int _activeTab = 0;
    private string? _capturedImageSource;

    // Declare ImageItems as a class-level property
    private UpdateUserProfileDto _user = new();

    public string Theme
    {
        get => _theme;
        set
        {
            _theme = value;
            OnPropertyChanged(nameof(Theme));
        }
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
    public void Setup(UpdateUserProfileDto updateUserProfileDto)
    {
        _user = updateUserProfileDto;
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
    private async void cameraPhotoButton_Clicked(object sender, EventArgs e)
    {
        if (!loadingDialog.IsVisible)
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null)
                {
                    CapturedImageSource = result.FullPath;
                    // Show thing a invisable 2
                    IsPhotoCaptured = true;
                    _isChangedAvatar = true;

                }
            }
            catch (Exception ex)
            {
                if (Application.Current != null && Application.Current.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                }
            }
        }
    }

    private async void galleryPhotoButton_Clicked(object sender, EventArgs e)
    {
        if (!loadingDialog.IsVisible)
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    CapturedImageSource = result.FullPath;
                    // Show thing a invisable 2
                    IsPhotoCaptured = true;
                    _isChangedAvatar = true;

                }
            }
            catch (Exception ex)
            {
                if (Application.Current != null && Application.Current.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                }
            }
        }
    }

    private void Undo_Clicked(object sender, EventArgs e)
    {
        // Show thing a invisable 1
        _isChangedAvatar = false;
        IsPhotoCaptured = false;
    }

   
    private void btnClose_Clicked(object sender, EventArgs e)
    {
        if (!loadingDialog.IsVisible)
            Hide();
    }

    private async void btnUploadAvatar_Clicked(object sender, EventArgs e)
    {
        if (Application.Current != null && Application.Current.MainPage != null && !loadingDialog.IsVisible)
        {
            if (_isChangedAvatar && CapturedImageSource != null)
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Thay đổi ảnh đại diện", "Bạn có chắc chắn về hành động này không?", "Có", "Không");
                if (answer)
                {
                    loadingDialog.IsVisible = true;
                    try
                    {
                        var token = await SecureStorage.GetAsync("auth_token");
                        if (token == null)
                        {
                            throw new Exception("Lỗi xác thực");
                        }
                        var url = await MauiUtils.UploadImageAsync(new FileResult(CapturedImageSource), token);
                        if (url != null)
                        {
                            await Fetcher.PatchAsync("api/users/update-avatar", new RequestUpdateAvatarDto { ImageAvatarUrl = url.FileUrl }, token);
                            _user.AvatarUrl = url.FileUrl;
                            _isChangedAvatar = false;
                            Hide();
                            await Application.Current.MainPage.DisplayAlert("Thành công", "Đã cập nhật ảnh thành công", "OK");
                        }

                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                    }
                    loadingDialog.IsVisible = false;

                }
            }
        }
    }
}