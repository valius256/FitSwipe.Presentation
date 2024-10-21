using FitSwipe.Shared.Dtos.Medias;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Utils;

namespace FitSwipe.Mobile.Pages.ProfilePages;

public partial class EditMediaModal : ContentView
{
	private GetUserMediaDto _userMediaDto = new();
	public GetUserMediaDto UserMediaDto
	{
		get => _userMediaDto;
		set
		{
			_userMediaDto = value;
			OnPropertyChanged(nameof(UserMediaDto));
		}
	}
	public EditMediaModal()
	{
		InitializeComponent();
		Hide();
		BindingContext = this;
	}
	public void Setup(GetUserMediaDto getUserMediaDto)
	{
        UserMediaDto = getUserMediaDto;
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

    private async void btnUpdate_Clicked(object sender, EventArgs e)
    {
        if (Application.Current != null && Application.Current.MainPage != null && !loadingDialog.IsVisible)
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
                    await Fetcher.PutAsync("api/medias", new RequestUpdateUserMediaDto { 
                        Description = UserMediaDto.Description,
                        Id = UserMediaDto.Id
                    }, token);
                    Hide();
                    await Application.Current.MainPage.DisplayAlert("Thành công", "Đã cập nhật thành công", "OK");                   

                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xảy ra. Err : " + ex.Message, "OK");
                }
                loadingDialog.IsVisible = false;

                
            }
        }
    }

    private void btnClose_Clicked(object sender, EventArgs e)
    {
        Hide();
    }
}