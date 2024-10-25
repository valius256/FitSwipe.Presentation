using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Utils;
using Syncfusion.Maui.Core.Carousel;

namespace FitSwipe.Mobile.Pages.ProfilePages;

[QueryProperty(nameof(PassedFlag), "flag")]
public partial class UserProfilePage : ContentPage
{
    public string? GuestId;
    public bool PassedFlag { get; set; } = false;
    private bool _isOwner { get; set; } = true;
    private string _token = string.Empty;
    private UserProfileViewModel viewModel;

    public UserProfilePage()
	{
		InitializeComponent();
		viewModel = new UserProfileViewModel(pageContent,loadingDialog,tagModal);
        SetIsOwner(true);

    }
    public UserProfilePage(string guestId)
    {
        InitializeComponent();
        viewModel = new UserProfileViewModel(pageContent, loadingDialog, tagModal, guestId);
        SetIsOwner(false);

    }
    protected override async void OnAppearing()
    {
        var currentToken = await SecureStorage.GetAsync("auth_token") ?? string.Empty;
        if (Helper.CheckTokenChanged(_token, currentToken))
        {
            _token = currentToken;
            viewModel.Setup();
            return;
        }
        if (PassedFlag)
        {
            await viewModel.FetchData();
            PassedFlag = false;
        }
    }
    public void SetIsOwner(bool isOwner)
    {
        _isOwner = isOwner;
        viewModel.IsOwner = _isOwner;
        if (_isOwner)
        {
            btnComeback.IsVisible = false;
            btnTalk.IsVisible = false;
        }
        else
        {
            btnEditAvatarSection.IsVisible = false;
            btnEditBioSection.IsVisible = false;
            btnNameEditSection.IsVisible = false;
            btnInfoEditSection.IsVisible = false;
            btnAddMedia.IsVisible = false;
            profileNavbar.IsVisible = false;
            navbar.IsVisible = false;
        }
        BindingContext = viewModel;
    }

    private void btnSaveName_Clicked(object sender, EventArgs e)
    {
    }

    private void btnCancelName_Clicked(object sender, EventArgs e)
    {
        viewModel.IsEditName = false;
        btnNameEditSection.IsVisible = true;
        lblUsername.IsVisible = true;
        editName.IsVisible = false;
    }

    private void btnSaveInfo_Clicked(object sender, EventArgs e)
    {

    }

    private void btnCencelInfo_Clicked(object sender, EventArgs e)
    {
        viewModel.IsEditPersonalInformation = false;
        btnInfoEditSection.IsVisible = true;
        lblDob.IsVisible = true;
        lblJob.IsVisible = true;
        lblAddress.IsVisible = true;
        editJob.IsVisible = false;
        editDob.IsVisible = false;
        editAddressSection.IsVisible = false;

    }

    private void btnSaveBio_Clicked(object sender, EventArgs e)
    {

    }
    private void btnCancelBio_Clicked(object sender, EventArgs e)
    {
        viewModel.IsEditBio = false;
        btnEditBioSection.IsVisible = true;
        lblBio.IsVisible = true;
        editBio.IsVisible = false;
    }

    private void tapEditInfor_Tapped(object sender, TappedEventArgs e)
    {
        btnInfoEditSection.IsVisible = false;
        viewModel.IsEditPersonalInformation = true;
        lblDob.IsVisible = false;
        lblJob.IsVisible = false;
        lblAddress.IsVisible = false;
        editJob.IsVisible = true;
        editDob.IsVisible = true;
        editAddressSection.IsVisible = true;
    }

    private void tapAvatar_Tapped(object sender, TappedEventArgs e)
    {
        traineeUploadMediaModal.Setup(viewModel.User);
        traineeUploadMediaModal.ActiveTab = 1;
        traineeUploadMediaModal.HandleSwitchTab();
        traineeUploadMediaModal.Show();
    }

    private void tapEditName_Tapped(object sender, TappedEventArgs e)
    {
        viewModel.IsEditName = true;
        btnNameEditSection.IsVisible = false;
        lblUsername.IsVisible = false;
        editName.IsVisible = true;
    }

    private void tapEditBio_Tapped(object sender, TappedEventArgs e)
    {
        viewModel.IsEditBio = true;
        btnEditBioSection.IsVisible = false;
        lblBio.IsVisible = false;
        editBio.IsVisible = true;
    }

    private void tagModal_OnConfirmed(object sender, Extensions.TagCheckedEventArgs e)
    {
        tagModal.Hide();
        viewModel.UpsertTags(e.Tags);
    }

    private void btnAddMedia_Clicked(object sender, EventArgs e)
    {
        traineeUploadMediaModal.Setup(viewModel.User);
        traineeUploadMediaModal.ActiveTab = 1;
        traineeUploadMediaModal.HandleSwitchTab();
        traineeUploadMediaModal.Show();
    }

    private async void btnComeback_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void btnSeemore_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//TraineeSchedulePage?flag=false");
    }

    private async void btnTalk_Clicked(object sender, EventArgs e)
    {
        if (!loadingDialog.IsVisible)
        {
            loadingDialog.IsVisible = true;
            loadingDialog.Message = "Vui lòng chờ...";
            try
            {
                await Shortcut.CreateChatRoomSolo(_token, viewModel.User.FireBaseId);
                await Shell.Current.GoToAsync($"//ChatPage?role=PT&flag=true&openId={viewModel.User.FireBaseId}");
            }
            catch
            {
                await Shell.Current.GoToAsync($"//ChatPage?role=PT&flag=false&openId={viewModel.User.FireBaseId}");

            }
            loadingDialog.IsVisible = false;
        }
    }
}