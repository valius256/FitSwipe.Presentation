using FitSwipe.Shared.Dtos.Users;

namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class SetupProfileStepFinalPage : ContentPage
{
    private string _mainColor1 = "LimeGreen";
    private string _mainColor2 = "LightGreen";
    private UpdateUserProfileDto _updateUserProfileDto;
    private List<Guid> _selectedTags;
    public string MainColor1
    {
        get => _mainColor1;
        set
        {
            _mainColor1 = value;
            OnPropertyChanged(nameof(MainColor1));
        }
    }
    public string MainColor2
    {
        get => _mainColor2;
        set
        {
            _mainColor2 = value;
            OnPropertyChanged(nameof(MainColor2));
        }
    }
    public SetupProfileStepFinalPage(UpdateUserProfileDto updateUserProfileDto, List<Guid> selectedTags)
	{
		InitializeComponent();
        _updateUserProfileDto = updateUserProfileDto; 
        _selectedTags = selectedTags;
	}

    private async void btnReady_Clicked(object sender, EventArgs e)
    {
        string message = $"Your current info : Bio : {_updateUserProfileDto.Bio},\n Job : {_updateUserProfileDto.Job},\n DateOfBirh : {_updateUserProfileDto.DateOfBirth},\n " +
            $"City : {_updateUserProfileDto.City},\n District : {_updateUserProfileDto.District},\n Ward : {_updateUserProfileDto.Ward},\n Gender : {_updateUserProfileDto.SelectedGender}";
        foreach (var tag in _selectedTags)
        {
            message += "\n Selected : " + tag.ToString();
        }
        await DisplayAlert("Review", message, "Ok baby");
        await Shell.Current.GoToAsync("//PTList");
    }

    private async void btnPrev_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}