using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class SetupProfileStep6Page : ContentPage
{
    private UpdateUserProfileDto _currentUser;
    private string _mainColor1 = "LimeGreen";
    private string _mainColor2 = "LightGreen";
    private string _question = "";
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
    public string Question
    {
        get => _question;
        set
        {
            _question = value;
            OnPropertyChanged(nameof(Question));
        }
    }
    public SetupProfileStep6Page(UpdateUserProfileDto updateUserProfileModel, List<Guid> alreadyTags)
    {
        InitializeComponent();
        FetchHobbyTags();
        _currentUser = updateUserProfileModel;
        AlreadyTags = alreadyTags;
        if (_currentUser.Role == Role.PT)
        {
            MainColor1 = "#2E3192";
            MainColor2 = "#1f00b8";
            Question = "Bạn muốn khách hàng của mình là người như thế nào?";
        } else
        {
            Question = "Hãy mô tả bản thân bạn";
        }

        BindingContext = this;
    }
    private void FetchHobbyTags()
    {
        //Test data, replace by fetching in the future
        Tags = new ObservableCollection<GetTagDto>()
        {
            new GetTagDto {Id = new Guid(), Name = "Rảnh rỗi", TagImage="Images/dotnet_bot.png",TagType = TagType.SelfDescription},
            new GetTagDto {Id = new Guid(), Name = "Kiên trì", TagImage="Images/dotnet_bot.png",TagType = TagType.SelfDescription},
            new GetTagDto {Id = new Guid(), Name = "Siêng năng", TagImage="Images/dotnet_bot.png",TagType = TagType.SelfDescription},
            new GetTagDto {Id = new Guid(), Name = "Hay học hỏi", TagImage="Images/dotnet_bot.png",TagType = TagType.SelfDescription},
            new GetTagDto {Id = new Guid(), Name = "Năng động", TagImage="Images/dotnet_bot.png",TagType = TagType.SelfDescription},
            new GetTagDto {Id = new Guid(), Name = "Rụt rè", TagImage="Images/dotnet_bot.png",TagType = TagType.SelfDescription},
            new GetTagDto {Id = new Guid(), Name = "Hơi lười", TagImage="Images/dotnet_bot.png",TagType = TagType.SelfDescription},
        };
        foreach (var tag in Tags)
        {
            tag.TagColor = GetRandomColor();
        }
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
        Navigation.PushModalAsync(new SetupProfileStepFinalPage(_currentUser, AlreadyTags));
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