using CommunityToolkit.Mvvm.ComponentModel;
using FitSwipe.Shared.Dtos;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class EditWorkoutSessionDetailViewModel : ObservableObject
  {
    public ObservableCollection<Video> Videos { get; set; }
    public ICommand ChangeCarouselPositionCommand { get; }
    public ICommand NavigateLeftCommand { get; }
    public ICommand NavigateRightCommand { get; }

    [ObservableProperty]
    private string selectedThumbnail;

    [ObservableProperty]
    private Video selectedVideo;

    [ObservableProperty]
    private int carouselPosition;

    [ObservableProperty]
    private Video selectedThumbnailVideo; // New property to track the selected thumbnail

    public EditWorkoutSessionDetailViewModel ()
    {
      Videos = new ObservableCollection<Video>()
            {
                new Video
                {
                    VideoSource = "https://storage.googleapis.com/fit-swipe-161d7.appspot.com/videos/719ZkC7AKbYDxU3l1dpkxvKqG3H2/354849733_6982381381780275_2790533324940314360_n.mp4",
                    ThumbnailSource = "https://scontent.xx.fbcdn.net/v/t1.15752-9/355040793_259496903327765_2250752017545809869_n.png?_nc_cat=105&ccb=1-7&_nc_sid=0024fc&_nc_eui2=AeFWIdqK8j8eVscLc8mzanixyN2gcYzbhxPI3aBxjNuHE-vg5ogaE1Kei2SDQ56wInUQem5ZpiSepikZAOJVVAyl&_nc_ohc=eWECrjci1f0Q7kNvgFcXl8B&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent.xx&_nc_gid=A3thyrOJRKcOr59pUAg9l6r&oh=03_Q7cD1QH4ODdT7PIOWlfWqMHwhUrVAAYn6RiqRnbdDTXp8acbqw&oe=6719108B",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris. Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris. Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris. Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris "
                },
                new Video
                {
                    VideoSource = "https://storage.googleapis.com/fit-swipe-161d7.appspot.com/videos/719ZkC7AKbYDxU3l1dpkxvKqG3H2/404568321_7252601748188284_6263480793654348234_n.mp4",
                    ThumbnailSource = "https://scontent.xx.fbcdn.net/v/t1.15752-9/344313296_970403757648062_993748438218919599_n.jpg?_nc_cat=100&ccb=1-7&_nc_sid=0024fc&_nc_eui2=AeHkRpKxreKfZSzIo_do23Rr1WGB3sqI_iHVYYHeyoj-IU7iO9mwltKkRGI9XXR2BKbOLq9CZHZpn8fLvHFUwDxn&_nc_ohc=v-dcW18xGzIQ7kNvgESR0dI&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent.xx&_nc_gid=AF7_elesra83Rqv7boTY3Ef&oh=03_Q7cD1QEr8rHBlmmJYZKqKjoDalFlf6QhgkDO9vnOX8Xkq7s1Jw&oe=671919C9",
                    Description = "Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris "
                }
            };

      // Initialize commands
      ChangeCarouselPositionCommand = new Command<Video>(OnChangeCarouselPosition);
      NavigateLeftCommand = new Command(OnNavigateLeft);
      NavigateRightCommand = new Command(OnNavigateRight);

      // Initialize selected video
      if (Videos.Count > 0)
      {
        SelectedVideo = Videos[0];
        CarouselPosition = 0;
        SelectedThumbnail = Videos[0].ThumbnailSource; // Set the selected thumbnail to the first element
        SelectedThumbnailVideo = SelectedVideo; // Set the initially selected thumbnail video
      }
    }

    partial void OnSelectedVideoChanged (Video value)
    {
      if (value != null)
      {
        SelectedThumbnail = value.ThumbnailSource; // Update selected thumbnail
        CarouselPosition = Videos.IndexOf(value);  // Update carousel position

        // Update the selected thumbnail video
        SelectedThumbnailVideo = value; // Set the selected thumbnail video
      }
    }

    private void OnChangeCarouselPosition (Video selectedVideo)
    {
      if (selectedVideo == null)
        return;

      var index = Videos.IndexOf(selectedVideo);

      if (index != -1)
      {
        SelectedVideo = selectedVideo; // This triggers OnSelectedVideoChanged
      }
    }

    private void OnNavigateLeft ()
    {
      if (CarouselPosition > 0)
      {
        CarouselPosition--;
        SelectedVideo = Videos[CarouselPosition];
      }
    }

    private void OnNavigateRight ()
    {
      if (CarouselPosition < Videos.Count - 1)
      {
        CarouselPosition++;
        SelectedVideo = Videos[CarouselPosition];
      }
    }
  }
}
