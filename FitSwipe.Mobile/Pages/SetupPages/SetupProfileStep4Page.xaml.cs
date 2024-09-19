using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class SetupProfileStep4Page : ContentPage
{
    private UpdateUserProfileDto _currentUser;
    private string _mainColor1 = "LimeGreen";
    private string _mainColor2 = "LightGreen";
    public ObservableCollection<GetTagDto> Tags { get; set; } = new ObservableCollection<GetTagDto>();

    public List<Guid> AlreadyTags;
    public List<Guid> NewTags = [];
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
    public SetupProfileStep4Page(UpdateUserProfileDto updateUserProfileModel, List<Guid> alreadyTags)
    {
        InitializeComponent();
        FetchHobbyTags();
        _currentUser = updateUserProfileModel;
        AlreadyTags = alreadyTags;
        BindingContext = this;
    }
    private void FetchHobbyTags()
    {
        //Test data, replace by fetching in the future
        Tags = new ObservableCollection<GetTagDto>()
        {
            new GetTagDto {Id = new Guid(), Name = "Tập sức mạnh", TagImage="Images/dotnet_bot.png",TagType = TagType.TrainingType},
            new GetTagDto {Id = new Guid(), Name = "Yoga", TagImage="Images/dotnet_bot.png",TagType = TagType.TrainingType},
            new GetTagDto {Id = new Guid(), Name = "Calisthenic", TagImage="Images/dotnet_bot.png",TagType = TagType.TrainingType},
            new GetTagDto {Id = new Guid(), Name = "Cardio", TagImage="Images/dotnet_bot.png",TagType = TagType.TrainingType},
            new GetTagDto {Id = new Guid(), Name = "HIIT", TagImage="Images/dotnet_bot.png",TagType = TagType.TrainingType},
        };
        foreach (var tag in Tags)
        {
            tag.DisplaySize = Math.Min(20, 20 / Math.Max(1, tag.Name.Length) * 20);
        }
    }
    private void btnPrev_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void btnNext_Clicked(object sender, EventArgs e)
    {
        NewTags.ForEach(t => AlreadyTags.Add(t));
        Navigation.PushModalAsync(new SetupProfileStep5Page(_currentUser, AlreadyTags));
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