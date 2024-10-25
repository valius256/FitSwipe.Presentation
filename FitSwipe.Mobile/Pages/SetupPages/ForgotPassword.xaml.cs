using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Medias;
using FitSwipe.Shared.Utils;

namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class ForgotPassword : ContentPage
{
    public string Email { get; set; } = string.Empty;
	public ForgotPassword()
	{
		InitializeComponent();
        BindingContext = this;
	}

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void btnSubmit_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Xác nhận", "Bạn có chắc chắn sử dụng email này để khôi phục mật khẩu không?", "Có", "Không");
        if (answer)
        {
            loadingDialog.IsVisible = true;
            try
            {
                
                await Fetcher.PostAsync("api/Authentication/forgot-password?email=" + Email, new BlankDto());
                await DisplayAlert("Thành công", "Chúng tôi đã gửi cho bạn 1 email thông qua địa chỉ bạn cung cấp. Vui lòng kiểm trả hòm thư và nhấp vào đường link trong đó để tiến hành khôi phục mật khẩu", "OK");
                await Navigation.PopModalAsync();

            }
            catch
            {
                await DisplayAlert("Lỗi", "Chúng tôi không tìm thấy email đó. Vui lòng kiểm tra lại hoặc thử email khác.", "OK");
            }
            loadingDialog.IsVisible = false;


        }
    }
}