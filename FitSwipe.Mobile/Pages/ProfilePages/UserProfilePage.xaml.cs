using FitSwipe.Mobile.ViewModels;

namespace FitSwipe.Mobile.Pages.ProfilePages;

public partial class UserProfilePage : ContentPage
{
    private bool _isOwner { get; set; } = true;
    private UserProfileViewModel viewModel;

    public UserProfilePage()
	{
		InitializeComponent();
		viewModel = new UserProfileViewModel();
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
            btnEditBioSection.IsVisible = false;
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
        editJob.IsVisible = false;
        editDob.IsVisible = false;

    }

    private void btnInfoEdit_Clicked(object sender, EventArgs e)
    {
        btnInfoEditSection.IsVisible = false;
        viewModel.IsEditPersonalInformation = true;
        lblDob.IsVisible = false;
        lblJob.IsVisible = false;
        editJob.IsVisible = true;
        editDob.IsVisible = true;
        

    }

    private void btnNameEdit_Clicked(object sender, EventArgs e)
    {
        viewModel.IsEditName = true;
        btnNameEditSection.IsVisible = false;
        lblUsername.IsVisible = false;
        editName.IsVisible = true;

    }

    private void btnEditBio_Clicked(object sender, EventArgs e)
    {
        viewModel.IsEditBio = true;
        btnEditBioSection.IsVisible = false;
        lblBio.IsVisible = false;
        editBio.IsVisible = true;
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
}