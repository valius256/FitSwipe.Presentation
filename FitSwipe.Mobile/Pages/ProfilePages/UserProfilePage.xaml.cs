using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages.ProfilePages;

public partial class UserProfilePage : ContentPage
{
    private bool _isOwner { get; set; } = true;
    private UserProfileViewModel viewModel;

    public UserProfilePage()
	{
		InitializeComponent();
		viewModel = new UserProfileViewModel(pageContent,loadingDialog,tagModal);
        SetIsOwner(true);

    }
    public void SetIsOwner(bool isOwner)
    {
        _isOwner = isOwner;
        viewModel.IsOwner = _isOwner;
        if (_isOwner)
        {
            btnComeback.IsVisible = false;
        }
        else
        {
            btnEditBioSection.IsVisible = false;
            btnNameEditSection.IsVisible = false;
            btnInfoEditSection.IsVisible = false;
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
}