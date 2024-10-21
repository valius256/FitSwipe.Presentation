using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Mobile.Utils;
using FitSwipe.Shared.Dtos.Medias;
using FitSwipe.Shared.Dtos.Upload;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FitSwipe.Mobile.Pages.ProfilePages;

public partial class PTUploadMediaModal : ContentView
{
    private bool _isPhotoCaptured = false;
    private bool _isDegreeCaptured = false;
    private bool _isChangedAvatar = false;
    private bool _isChangedDegree = false;
    private int _activeTab = 0;
    private string? _capturedImageSource;
    private string? _capturedDegreeSource;

    // Declare ImageItems as a class-level property
    private ObservableCollection<GetUserMediaDto> _medias { get; set; } = [];
    private GetUserDetailDto _userDetail = new();

    public PTUploadMediaModal()
    {
        InitializeComponent();
        HandleSwitchTab();
        Hide();
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
    public bool IsDegreeCaptured
    {
        get => _isDegreeCaptured;
        set
        {
            _isDegreeCaptured = value;
            OnPropertyChanged();
        }
    }
    public bool IsChangedAvatar
    {
        get => _isDegreeCaptured;
        set
        {
            _isChangedAvatar = value;
            OnPropertyChanged(nameof(IsChangedAvatar));
        }
    }
    public bool IsChangedDegree
    {
        get => _isChangedDegree;
        set
        {
            _isChangedDegree = value;
            OnPropertyChanged(nameof(IsChangedDegree));
        }
    }
    public string? CapturedDegreeSource
    {
        get => _capturedDegreeSource;
        set
        {
            _capturedDegreeSource = value;
            OnPropertyChanged(nameof(CapturedDegreeSource));
        }
    }
    public ObservableCollection<GetUserMediaDto> Medias
    {
        get => _medias;
        set
        {
            _medias = value;
            OnPropertyChanged(nameof(Medias));
        }
    }
    public void Setup(GetUserDetailDto userDetailDto)
    {
        _userDetail = userDetailDto;
        Medias = userDetailDto.UserMedias;
        if (userDetailDto.AvatarUrl != null)
        {
            CapturedImageSource = userDetailDto.AvatarUrl;
            IsPhotoCaptured = true;
        }
        if (userDetailDto.PTDegreeImageUrl != null)
        {
            CapturedDegreeSource = userDetailDto.PTDegreeImageUrl;
            IsDegreeCaptured = true;
        }
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
                    //var stream = await result.OpenReadAsync();
                    //var imageSource = ImageSource.FromStream(() => stream);
                    if (ActiveTab == 0)
                    {

                        CapturedImageSource = result.FullPath;
                        // Show thing a invisable 2
                        IsPhotoCaptured = true;
                        _isChangedAvatar = true;
                    }
                    else if (ActiveTab == 1)
                    {
                        await AddUserMedia(new GetUserMediaDto { MediaUrl = result.FullPath, IsVideo = false });
                    } 
                    else
                    {
                        CapturedDegreeSource = result.FullPath;
                        // Show thing a invisable 2
                        IsDegreeCaptured = true;
                        IsChangedDegree = true;
                    }

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
                    //var stream = await result.OpenReadAsync();
                    //var imageSource = ImageSource.FromStream(() => stream);
                    if (ActiveTab == 0)
                    {

                        CapturedImageSource = result.FullPath;
                        // Show thing a invisable 2
                        IsPhotoCaptured = true;
                        IsChangedAvatar = true;
                    }
                    else if (ActiveTab == 1)
                    {
                        await AddUserMedia(new GetUserMediaDto { MediaUrl = result.FullPath, IsVideo = false });
                    } else
                    {
                        CapturedDegreeSource = result.FullPath;
                        // Show thing a invisable 2
                        IsDegreeCaptured = true;
                        IsChangedDegree = true;
                    }

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
    private async Task AddUserMedia(GetUserMediaDto media)
    {
        if (Application.Current != null && Application.Current.MainPage != null && !loadingDialog.IsVisible)
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Thêm ảnh/ video", "Bạn có chắc chắn về hành động này không?", "Có", "Không");
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
                    GetUploadResultDto? url;
                    if (media.IsVideo)
                    {
                        url = await MauiUtils.UploadVideoAsync(new FileResult(media.MediaUrl), token);
                    } else
                    {
                        url = await MauiUtils.UploadImageAsync(new FileResult(media.MediaUrl), token);
                    }
                    if (url != null)
                    {
                        var addedMedia = await Fetcher.PostAsync<RequestCreateUserMediaDto,GetUserMediaDto>("api/medias", new RequestCreateUserMediaDto { 
                            MediaUrl = url.FileUrl ,
                            IsVideo = media.IsVideo,
                            Description = media.Description,
                        }, token);
                        if (addedMedia != null)
                        {
                            Medias.Add(addedMedia);
                            await Application.Current.MainPage.DisplayAlert("Thành công", "Đã đăng ảnh / video thành công", "OK");
                        }
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

    private void NextPageUpload_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new UserUploadImageVideo());
        ActiveTab = 1;
        HandleSwitchTab();

    }
    private void Undo_Clicked(object sender, EventArgs e)
    {
        // Show thing a invisable 1
        _isChangedAvatar = false;
        IsPhotoCaptured = false;
    }

    private void btnTabAvatar_Clicked(object sender, EventArgs e)
    {
        ActiveTab = 0;
        HandleSwitchTab();

    }

    public void HandleSwitchTab()
    {
        switch (ActiveTab)
        {
            case 0:
                tab1Content.IsVisible = true;
                tab2Content.IsVisible = false;
                tab3Content.IsVisible = false;
                btnTabMedia.BackgroundColor = Colors.White;
                btnTabAvatar.BackgroundColor = Color.FromArgb("#FF2E3192");
                btnDegree.BackgroundColor = Colors.White;
                btnTabAvatar.TextColor = Colors.White;
                btnTabMedia.TextColor = Colors.Black;
                btnDegree.TextColor = Colors.Black;
                btnTabAvatar.ZIndex = 2;
                btnTabMedia.ZIndex = 1;
                btnDegree.ZIndex = 0;
                break;
            case 1:
                btnTabAvatar.BackgroundColor = Colors.White;
                btnTabMedia.BackgroundColor = Color.FromArgb("#FF2E3192");
                btnDegree.BackgroundColor = Colors.White;

                btnTabAvatar.TextColor = Colors.Black;
                btnTabMedia.TextColor = Colors.White;
                btnDegree.TextColor = Colors.Black;

                tab1Content.IsVisible = false;
                tab2Content.IsVisible = true;
                tab3Content.IsVisible = false;

                btnTabAvatar.ZIndex = 0;
                btnTabMedia.ZIndex = 1;
                btnDegree.ZIndex = 0;

                break;
            case 2:
                btnTabAvatar.BackgroundColor = Colors.White;
                btnDegree.BackgroundColor = Color.FromArgb("#FF2E3192");
                btnTabMedia.BackgroundColor = Colors.White;

                btnTabAvatar.TextColor = Colors.Black;
                btnDegree.TextColor = Colors.White;
                btnTabMedia.TextColor = Colors.Black;

                tab1Content.IsVisible = false;
                tab2Content.IsVisible = false;
                tab3Content.IsVisible = true;

                btnTabAvatar.ZIndex = 0;
                btnTabMedia.ZIndex = 1;
                btnDegree.ZIndex = 2;

                break;
        }
    }

    private async void btnRemove_Clicked(object sender, EventArgs e)
    {
        if (Application.Current != null && Application.Current.MainPage != null && !loadingDialog.IsVisible)
        {
            var button = sender as ImageButton;
            if (button != null && !loadingDialog.IsVisible)
            {
                var boundItem = button.CommandParameter as GetUserMediaDto;
                if (boundItem != null)
                {
                    var answer = await Application.Current.MainPage.DisplayAlert("Thêm ảnh/ video", "Bạn có chắc chắn về hành động này không?", "Có", "Không");
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
                            await Fetcher.DeleteAsync($"api/medias/{boundItem.Id}", token);
                            Medias.Remove(boundItem);
                            await Application.Current.MainPage.DisplayAlert("Thành công", "Đã xóa ảnh / video thành công", "OK");                          
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
                            _userDetail.AvatarUrl = url.FileUrl;
                            _isChangedAvatar = false;
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

    private void tapImge_Tapped(object sender, TappedEventArgs e)
    {
        var frame = sender as Frame;
        if (frame != null)
        {
            var tapGesture = frame.GestureRecognizers[0] as TapGestureRecognizer;
            if (tapGesture != null)
            {
                var boundItem = tapGesture.CommandParameter as GetUserMediaDto;
                if (boundItem != null)
                {
                    editMediaModal.Setup(boundItem);
                    editMediaModal.Show();
                }              
            }
        }
        
    }

    private async void tapPickVideo_Tapped(object sender, TappedEventArgs e)
    {
        if (!loadingDialog.IsVisible)
        {
            try
            {
                var result = await MediaPicker.PickVideoAsync();
                if (result != null)
                {
                    await AddUserMedia(new GetUserMediaDto { MediaUrl = result.FullPath, IsVideo = true });
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

    private void btnDegree_Clicked(object sender, EventArgs e)
    {
        ActiveTab = 2;
        HandleSwitchTab();
    }

    private async void btnUploadDegree_Clicked(object sender, EventArgs e)
    {
        if (Application.Current != null && Application.Current.MainPage != null && !loadingDialog.IsVisible)
        {
            if (_isChangedDegree && CapturedDegreeSource != null)
            {
                loadingDialog.IsVisible = true;
                try
                {
                    var token = await SecureStorage.GetAsync("auth_token");
                    var url = await MauiUtils.UploadImageAsync(new FileResult(CapturedDegreeSource), token ?? string.Empty);
                    if (url == null)
                    {
                        throw new Exception("Lỗi đăng ảnh");
                    }
                    await Fetcher.PatchAsync("api/users/update-degree", new UpdateImageUrlDto { Url = url.FileUrl }, token ?? string.Empty);                   
                    await Application.Current.MainPage.DisplayAlert("Thành công", "Đã cập nhật thành công", "OK");
                    _userDetail.PTDegreeImageUrl = url.FileUrl;
                    IsChangedDegree = false;
                }
                catch (Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Đăng ảnh thất bại, vui lòng thừ lại sau", "OK");
                }
                loadingDialog.IsVisible = false;
            }
        }
    }
}


