using CommunityToolkit.Mvvm.ComponentModel;
using FitSwipe.Mobile.ViewModels.UserProfile;
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Users;
using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class UserProfileViewModel : ObservableObject
  {
    [ObservableProperty]
    private ObservableCollection<GetTagDto> userTags = new();

    [ObservableProperty]
    private GetUserDto user = new();
    public UserProfileCombinedViewModel CombinedViewModel { get; set; }

    [ObservableProperty]
    private UserImage selectedImage;

    // Property to track the currently selected image in the carousel
    [ObservableProperty]
    private UserImage currentImage;

    // Constructor to initialize with sample data
    public UserProfileViewModel ()
    {
      user = new GetUserDto()
      {
        DateOfBirth = new DateTime(2024, 9, 21),
        Ward = "Phườn 16",
        District = "Đường Hoàng Ngân",
        City = "Hồ Chí Minh",
        Job = "Sinh viên",
        ImageCollection = new List<UserImage>
        {
          new UserImage { ImageSource = "pt1.jpg", Description="nhìn cái cơ bắp này đi"},
          new UserImage { ImageSource = "pt2.jpg", Description="chào em, anh đứng đây từ chiều"},
          new UserImage { ImageSource = "pt3.jpg", Description="tôi đô không em?"},
          new UserImage { ImageSource = "profile_thumbnail1.png", Description = "never gonna give you up 🗣️🗣️"},
          new UserImage { ImageSource = "profile_thumbnail2.png", Description = "a du ang seng"},
        }
      };

      // Initialize the userTags collection with sample data and different colors
      userTags = new ObservableCollection<GetTagDto>
            {
                new GetTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Strength",
                    TagType = TagType.Target,
                    TagColor = "#52BB00", // Green
                    CreateById = "User1",
                    TagImage = "strength_icon.png",
                    SpecialTag = SpecialTag.DifferentGender,
                    CreatedDate = DateTime.Now,
                    RecordStatus = RecordStatus.Active,
                    DisplaySize = 12,
                    IsSelected = false
                },
                new GetTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Cardio",
                    TagType = TagType.Hobby,
                    TagColor = "#FF5733", // Red-Orange
                    CreateById = "User2",
                    TagImage = "cardio_icon.png",
                    SpecialTag = SpecialTag.DifferentGender,
                    CreatedDate = DateTime.Now,
                    RecordStatus = RecordStatus.Active,
                    DisplaySize = 10,
                    IsSelected = true
                },
                new GetTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Flexibility",
                    TagType = TagType.SelfDescription,
                    TagColor = "#3355FF", // Blue
                    CreateById = "User3",
                    TagImage = "flexibility_icon.png",
                    SpecialTag = SpecialTag.Close,
                    CreatedDate = DateTime.Now,
                    RecordStatus = RecordStatus.Active,
                    DisplaySize = 15,
                    IsSelected = false
                },
                new GetTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Running",
                    TagType = TagType.Hobby,
                    TagColor = "#FFA500", // Orange
                    CreateById = "User3",
                    TagImage = "running_icon.png",
                    SpecialTag = SpecialTag.Close,
                    CreatedDate = DateTime.Now,
                    RecordStatus = RecordStatus.Active,
                    DisplaySize = 15,
                    IsSelected = false
                },
                new GetTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Fishing",
                    TagType = TagType.Hobby,
                    TagColor = "#FFD700", // Gold
                    CreateById = "User3",
                    TagImage = "fishing_icon.png",
                    SpecialTag = SpecialTag.Close,
                    CreatedDate = DateTime.Now,
                    RecordStatus = RecordStatus.Active,
                    DisplaySize = 15,
                    IsSelected = false
                },
                new GetTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Sweet Home Alabama",
                    TagType = TagType.Hobby,
                    TagColor = "#800080", // Purple
                    CreateById = "User3",
                    TagImage = "home_icon.png",
                    SpecialTag = SpecialTag.Close,
                    CreatedDate = DateTime.Now,
                    RecordStatus = RecordStatus.Active,
                    DisplaySize = 15,
                    IsSelected = false
                },
                new GetTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Weight Lifting",
                    TagType = TagType.TrainingType,
                    TagColor = "#ffc300",
                    CreateById = "User3",
                    TagImage = "home_icon.png",
                    SpecialTag = SpecialTag.Close,
                    CreatedDate = DateTime.Now,
                    RecordStatus = RecordStatus.Active,
                    DisplaySize = 15,
                    IsSelected = false
                },
                new GetTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Katarinanian Workout",
                    TagType = TagType.TrainingType,
                    TagColor = "#a71e34",
                    CreateById = "User3",
                    TagImage = "home_icon.png",
                    SpecialTag = SpecialTag.Close,
                    CreatedDate = DateTime.Now,
                    RecordStatus = RecordStatus.Active,
                    DisplaySize = 15,
                    IsSelected = false
                },
                new GetTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Buff",
                    TagType = TagType.PTTaste,
                    TagColor = "#023e8a",
                    CreateById = "User3",
                    TagImage = "home_icon.png",
                    SpecialTag = SpecialTag.Close,
                    CreatedDate = DateTime.Now,
                    RecordStatus = RecordStatus.Active,
                    DisplaySize = 15,
                    IsSelected = false
                },
                new GetTagDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Polite",
                    TagType = TagType.PTTaste,
                    TagColor = "#599079",
                    CreateById = "User3",
                    TagImage = "home_icon.png",
                    SpecialTag = SpecialTag.Close,
                    CreatedDate = DateTime.Now,
                    RecordStatus = RecordStatus.Active,
                    DisplaySize = 15,
                    IsSelected = false
                },
            };

      // Set the initial current image
      CurrentImage = User.ImageCollection.FirstOrDefault();

      // Set SelectedImage to the initial CurrentImage
      SelectedImage = CurrentImage;

      // Create the combined view model
      CombinedViewModel = new UserProfileCombinedViewModel(user, userTags);

      // Handle property changed for CurrentImage to update SelectedImage
      this.PropertyChanged += (s, e) =>
      {
        if (e.PropertyName == nameof(CurrentImage))
        {
          SelectedImage = CurrentImage;
        }
      };
    }

  }
}
