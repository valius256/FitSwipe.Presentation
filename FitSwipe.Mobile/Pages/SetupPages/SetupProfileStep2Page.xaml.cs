using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class SetupProfileStep2Page : ContentPage
{
    private UpdateUserProfileDto _currentUser;
    private string _mainColor1 = "LimeGreen";
    private string _mainColor2 = "LightGreen";
    public ObservableCollection<GetTagDto> Tags { get; set; } = new ObservableCollection<GetTagDto>();
    public List<Guid> SelectedTags { get; set; } = [];
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
    public SetupProfileStep2Page(UpdateUserProfileDto updateUserProfileModel)
    {
        InitializeComponent();
        FetchHobbyTags();
        _currentUser = updateUserProfileModel;
        BindingContext = this;
    }
    private void FetchHobbyTags()
    {
        //Test data, replace by fetching in the future
        Tags = new ObservableCollection<GetTagDto>()
        {
            new GetTagDto {Id = new Guid(), Name = "Đá bóng", TagImage="Images/dotnet_bot.png",TagType = TagType.Hobby},
            new GetTagDto {Id = new Guid(), Name = "Chơi game", TagImage="Images/dotnet_bot.png",TagType = TagType.Hobby},
            new GetTagDto {Id = new Guid(), Name = "Đọc sách", TagImage="Images/dotnet_bot.png",TagType = TagType.Hobby},
            new GetTagDto {Id = new Guid(), Name = "Chạy bộ", TagImage="Images/dotnet_bot.png",TagType = TagType.Hobby},
            new GetTagDto {Id = new Guid(), Name = "Xem phim", TagImage="Images/dotnet_bot.png",TagType = TagType.Hobby},
        };
        foreach (var tag in Tags)
        {
            tag.DisplaySize = Math.Min(20, 8 / Math.Max(1, tag.Name.Length) * 20);
        }
    }
    private void btnPrev_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void btnNext_Clicked(object sender, EventArgs e)
    {
        //DisplayAlert("Information",$"Current User : {_currentUser}, Selected Tag : {SelectedTags}","OK");
        Navigation.PushModalAsync(new SetupProfileStep3Page(_currentUser, SelectedTags));
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