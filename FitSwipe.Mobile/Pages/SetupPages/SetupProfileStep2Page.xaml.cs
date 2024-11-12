using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class SetupProfileStep2Page : ContentPage
{
    private UpdateUserProfileDto _currentUser;
    private string _mainColor1 = "LimeGreen";
    private string _mainColor2 = "LightGreen";
    private string _question = "Sở thích của bạn là gì?";
    private ObservableCollection<GetTagDto> _tags = new ObservableCollection<GetTagDto>();

    public List<Guid> SelectedTags { get; set; } = [];
    public ObservableCollection<GetTagDto> Tags
{
        get => _tags;
        set
        {
            _tags = value;
            OnPropertyChanged(nameof(Tags));
        }
    }
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
    public string Question
    {
        get => _question;
        set
        {
            _question = value;
            OnPropertyChanged(nameof(Question));
        }
    }
    public SetupProfileStep2Page(UpdateUserProfileDto updateUserProfileModel)
    {
        InitializeComponent();
        FetchHobbyTags();
        _currentUser = updateUserProfileModel;
        if (_currentUser.Role == Role.PT)
        {
            MainColor1 = "#2E3192";
            MainColor2 = "#1f00b8";
        }

        BindingContext = this;
    }
    private async void FetchHobbyTags()
    {
        pageContent.IsVisible = false;
        loadingDialog.IsVisible = true;
        try
        {
            var tags = await Fetcher.GetAsync<ObservableCollection<GetTagDto>>("api/tags?TagTypes=0");
            if (tags != null)
            {
                Tags = tags;
                foreach (var tag in Tags)
                {
                    tag.DisplaySize = Math.Min(20, 20 / Math.Max(1, tag.Name.Length) * 20);
                }
            }
        }
        catch (Exception)
        {
            await DisplayAlert("Lỗi", "Có lỗi đã xảy ra! Vui lòng thử lại sau", "OK");
        }
        pageContent.IsVisible = true;
        loadingDialog.IsVisible = false;
    }
    private void btnPrev_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private async void btnNext_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SetupProfileStep3Page(_currentUser, SelectedTags));

        //if (SelectedTags.Count > 0)
        //{
        //} else
        //{
        //    await DisplayAlert("Thiếu thông tin", "Hãy vui lòng chọn ít nhất 1 thẻ", "OK");
        //}
    }

    private void tagFrame_Tapped(object sender, TappedEventArgs e)
    {
        var frame = sender as Frame; // The sender is the Frame, not the TapGestureRecognizer

        if (frame != null)
        {
            var tapGestureRecognizer = frame.GestureRecognizers[0] as TapGestureRecognizer;
            if (tapGestureRecognizer != null)
            {
                // Retrieve the bound object via CommandParameter
                var boundItem = tapGestureRecognizer?.CommandParameter as GetTagDto;
                if (boundItem != null)
                {
                    if (boundItem.IsSelected)
                    {
                        boundItem.IsSelected = false;
                        SelectedTags.Remove(boundItem.Id);
                    }
                    else
                    {
                        boundItem.IsSelected = true;
                        SelectedTags.Add(boundItem.Id);
                    }
                }
            }
        }
    }


}