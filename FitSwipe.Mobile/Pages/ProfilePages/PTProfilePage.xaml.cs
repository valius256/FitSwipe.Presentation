using FitSwipe.Mobile.ViewModels.UserProfile;

namespace FitSwipe.Mobile.Pages.ProfilePages;

[QueryProperty(nameof(PassedFlag), "flag")]
public partial class PTProfilePage : ContentPage
{
    public bool PassedFlag { get; set; } = false;
    private bool _isOwner { get; set; } = true;
    private PTProfileViewModel viewModel;

    public PTProfilePage()
    {
        InitializeComponent();
        viewModel = new PTProfileViewModel(pageContent, loadingDialog, tagModal,timeTable);
        SetIsOwner(true);

    }
    public PTProfilePage(string guestId)
    {
        InitializeComponent();
        viewModel = new PTProfileViewModel(pageContent, loadingDialog, tagModal,timeTable, guestId);
        SetIsOwner(false);

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
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
        pTUploadMediaModal.Setup(viewModel.User);
        pTUploadMediaModal.ActiveTab = 1;
        pTUploadMediaModal.HandleSwitchTab();
        pTUploadMediaModal.Show();
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
        pTUploadMediaModal.Setup(viewModel.User);
        pTUploadMediaModal.ActiveTab = 1;
        pTUploadMediaModal.HandleSwitchTab();
        pTUploadMediaModal.Show();
    }

    private void timeTable_SlotAction(object sender, Extensions.SlotEventArgs e)
    {

    }

    private async void timeTable_WeekChanged(object sender, EventArgs e)
    {
        await viewModel.FetchSlots(false);
    }

    private async void btnMoreRate_Clicked(object sender, EventArgs e)
    {
        if (!viewModel.IsFetchingComment)
        {
            if (viewModel.CurrentRatingPage >= viewModel.TotalRatingPage)
            {
                showMoreRatingSection.IsVisible = false;
                return;
            }
            await viewModel.FetchRatingData(false);
            if (viewModel.CurrentRatingPage == viewModel.TotalRatingPage)
            {
                showMoreRatingSection.IsVisible = false;
            }
        }
    }

    private async void btnComeback_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}