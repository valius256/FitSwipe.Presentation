using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class SetupProfileStep5Page : ContentPage
{
    private UpdateUserProfileDto _currentUser;
    private string _mainColor1 = "LimeGreen";
    private string _mainColor2 = "LightGreen";
    private string _question = "";
    private ObservableCollection<GetTagDto> _tags = new ObservableCollection<GetTagDto>();

    public List<Guid> AlreadyTags;
    public List<Guid> NewTags = [];

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
    public SetupProfileStep5Page(UpdateUserProfileDto updateUserProfileModel, List<Guid> alreadyTags)
    {
        InitializeComponent();
        FetchHobbyTags();
        _currentUser = updateUserProfileModel;
        AlreadyTags = alreadyTags;
        if (_currentUser.Role == Role.PT)
        {
            MainColor1 = "#2E3192";
            MainColor2 = "#1f00b8";
            Question = "Hãy mô tả bản thân bạn với tư cách là Huấn luyện viên";
        } else
        {
            Question = "Tiêu chí chọn PT của bạn là gì?";
        }

        BindingContext = this;
    }
    private async void FetchHobbyTags()
    {
        pageContent.IsVisible = false;
        loadingDialog.IsVisible = true;
        try
        {
            var tags = await Fetcher.GetAsync<ObservableCollection<GetTagDto>>("api/tags?TagTypes=3");
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
    private string GetRandomColor()
    {
        Random random = new Random();
        int red = random.Next(0, 256);
        int green = random.Next(0, 256);
        int blue = random.Next(0, 256);

        return $"#{red:X2}{green:X2}{blue:X2}";
        
    }
    private void btnPrev_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void btnNext_Clicked(object sender, EventArgs e)
    {
        NewTags.ForEach(t => AlreadyTags.Add(t));
        Navigation.PushModalAsync(new SetupProfileStep6Page(_currentUser, AlreadyTags));
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
                        NewTags.Remove(boundItem.Id);
                    }
                    else
                    {
                        boundItem.IsSelected = true;
                        NewTags.Add(boundItem.Id);
                    }
                }
            }
        }
    }
}