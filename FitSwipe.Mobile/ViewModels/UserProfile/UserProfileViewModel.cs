using CommunityToolkit.Mvvm.ComponentModel;
using FitSwipe.Mobile.ViewModels.UserProfile;
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Medias;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Trainings;
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
    private GetUserDetailDto user = new();
    //public UserProfileCombinedViewModel CombinedViewModel { get; set; }

    [ObservableProperty]
    private GetUserMediaDto? selectedImage;

    // Property to track the currently selected image in the carousel
    [ObservableProperty]
    private GetUserMediaDto? currentImage;

    [ObservableProperty]
    private GetTrainingDetailDto? currentTraining;

    [ObservableProperty]
    private string? currentTrainingSlotDisplay;

    [ObservableProperty]
    private bool isOwner = true;
    [ObservableProperty]
    private bool isShowTrainingSection = true;
    [ObservableProperty]
    private bool isEditName = false;
    [ObservableProperty]
    private bool isEditPersonalInformation = false;
    [ObservableProperty]
    private bool isEditBio = false;
    [ObservableProperty]
    private ObservableCollection<string> jobs = new();

    // Constructor to initialize with sample data
    public UserProfileViewModel ()
    {
      jobs = new ObservableCollection<string> { "Học sinh, Sinh viên", "Lao động chân tay", "Sư phạm" , "Y học" , "Kĩ sư" , "Kinh doanh", "Công nghệ thông tin",  "Làm thuê làm mướn" ,"Làm việc văn phòng","Hiện tại thất nghiệp", "Khác","Không muốn chia sẻ" };

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

      user = new GetUserDetailDto()
      {
        UserName = "Nguyen Thanh Phong",
        AvatarUrl = "pt5.png",
        DateOfBirth = new DateTime(2024, 9, 21),
        Ward = "Phườn 16",
        District = "Đường Hoàng Ngân",
        City = "Hồ Chí Minh",
        Job = "Sinh viên",
        Bio = "Anh không thích kem, chỉ thích 2/3 dưới của nó thoi!",
        UserMedias = new List<GetUserMediaDto>
        {
          new GetUserMediaDto { MediaUrl = "pt1.jpg", Description="nhìn cái cơ bắp này đi"},
          new GetUserMediaDto { MediaUrl = "pt2.jpg", Description="chào em, anh đứng đây từ chiều"},
          new GetUserMediaDto { MediaUrl = "pt3.jpg", Description="tôi đô không em?"},
          new GetUserMediaDto { MediaUrl = "profile_thumbnail1.png", Description = "never gonna give you up 🗣️🗣️"},
          new GetUserMediaDto { MediaUrl = "profile_thumbnail2.png", Description = "a du ang seng"},
        },
          // Initialize the userTags collection with sample data and different colors
          Tags = userTags,
      };
      CurrentTraining = new GetTrainingDetailDto()
      {
          PT = new GetUserDto { UserName = "Achang Đẹp Trai", AvatarUrl = "pt1.png"},
          Trainee = new GetUserDto { UserName = "Nguyen Thanh Phong", AvatarUrl = "pt5.png" },
          Slots = new ObservableCollection<GetSlotDto>
          {
              new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,20,7,0,0), EndTime = new DateTime(2024,9,20,8,30,0), Location = "Phòng tập City Gym" , TotalVideos = 5, Content="Push ups" },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,19,7,0,0), EndTime = new DateTime(2024,9,19,8,0,0), Location = "Phòng tập City Gym" , TotalVideos = 3, Content="Sit ups"  },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,18,10,15,0), EndTime = new DateTime(2024,9,18,12,45,0), Location = "Phòng tập City Gym" , TotalVideos = 5 , Content="Run 10km" },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,18,15,30,0), EndTime = new DateTime(2024,9,18,17,30,0), Location = "" , TotalVideos = 4, Content="Mix up"  },
            new GetSlotDto { Id = Guid.NewGuid(), StartTime =  new DateTime(2024,9,16,7,0,0), EndTime = new DateTime(2024,9,16,8,30,0) , Location = "" , TotalVideos = 5, Content="Pull ups"  }
          },
          Status = TrainingStatus.OnGoing
      };
      //Setup the training
      ColorTrainingStatus();


      // Set the initial current image
      CurrentImage = User.UserMedias.FirstOrDefault();

      // Set SelectedImage to the initial CurrentImage
      SelectedImage = CurrentImage;

      // Create the combined view model
      //CombinedViewModel = new UserProfileCombinedViewModel(user, userTags);

      // Handle property changed for CurrentImage to update SelectedImage
      PropertyChanged += (s, e) =>
      {
        if (e.PropertyName == nameof(CurrentImage))
        {
          SelectedImage = CurrentImage;
        }
      };
    }
    private void ColorTrainingStatus()
    {
        if (CurrentTraining != null)
        {
            if (CurrentTraining.Status == TrainingStatus.NotStarted)
            {
                    CurrentTraining.StatusString = "Sắp bắt đầu";
                    CurrentTraining.StatusColor = "#969696";
            }
            if (CurrentTraining.Status == TrainingStatus.OnGoing)
            {
                    CurrentTraining.StatusString = "Đang diễn ra";
                    CurrentTraining.StatusColor = "#52BB00";
            }
            double duration = 0;
            foreach (var item in CurrentTraining.Slots)
            {
                duration += (item.EndTime - item.StartTime).TotalHours;
            }
            CurrentTrainingSlotDisplay = $"{CurrentTraining.Slots.Count} buổi, {(int)duration} tiếng";
        }      
    }
  }
}
