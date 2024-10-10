using CommunityToolkit.Mvvm.ComponentModel;
using FitSwipe.Shared.Dtos;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Media;
using Microsoft.Maui.ApplicationModel;
using CommunityToolkit.Mvvm.Input;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class EditWorkoutSessionDetailViewModel : ObservableObject
  {
    public ObservableCollection<Media> Medias { get; set; } = new ObservableCollection<Media>();
    public ICommand ChangeCarouselPositionCommand { get; }
    public ICommand NavigateLeftCommand { get; }
    public ICommand NavigateRightCommand { get; }
    public ICommand AddMediaCommand { get; }
    public ICommand DeleteMediaCommand { get; }
    public ICommand ChangeThumbnailCommand { get; }

    [ObservableProperty]
    private string selectedThumbnail;
    [ObservableProperty]
    private Media selectedMedia;

    [ObservableProperty]
    private int carouselPosition;

    private bool _isUpdatingMedia; // Flag to prevent re-entrant UI updates
    private Media _previouslySelectedMedia; // Track the previously selected video

    public EditWorkoutSessionDetailViewModel ()
    {
      Medias.Add(new Media
      {
        Source = "https://storage.googleapis.com/fit-swipe-161d7.appspot.com/videos/719ZkC7AKbYDxU3l1dpkxvKqG3H2/354849733_6982381381780275_2790533324940314360_n.mp4",
        Thumbnail = "https://scontent.xx.fbcdn.net/v/t1.15752-9/355040793_259496903327765_2250752017545809869_n.png?_nc_cat=105&ccb=1-7&_nc_sid=0024fc&_nc_eui2=AeFWIdqK8j8eVscLc8mzanixyN2gcYzbhxPI3aBxjNuHE-vg5ogaE1Kei2SDQ56wInUQem5ZpiSepikZAOJVVAyl&_nc_ohc=eWECrjci1f0Q7kNvgFcXl8B&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent.xx&_nc_gid=A3thyrOJRKcOr59pUAg9l6r&oh=03_Q7cD1QH4ODdT7PIOWlfWqMHwhUrVAAYn6RiqRnbdDTXp8acbqw&oe=6719108B",
        Description = "Video 1 description"
      });

      ChangeCarouselPositionCommand = new Command<Media>(OnThumbnailTapped);
      NavigateLeftCommand = new Command(OnNavigateLeft);
      NavigateRightCommand = new Command(OnNavigateRight);
      AddMediaCommand = new Command(async () => await AddNewMedia());
      DeleteMediaCommand = new Command<Media>(DeleteMedia);
      ChangeThumbnailCommand = new RelayCommand<Media>(OnThumbnailTapped);

      // Initialize selected video
      if (Medias.Count > 0)
      {
        SelectedMedia = Medias[0];
        CarouselPosition = 0;
        SelectedThumbnail = Medias[0].Source;
        Medias[0].ThumbnailShowPlayIcon = true; // Show play icon for the first video
      }
    }

    partial void OnSelectedMediaChanged (Media value)
    {
      if (value != null && !_isUpdatingMedia) // Prevent re-entrant updates
      {
        try
        {
          _isUpdatingMedia = true; // Prevent recursive updates

          // Reset the previously selected video's play icon (if any)
          if (_previouslySelectedMedia != null && _previouslySelectedMedia != value)
          {
            _previouslySelectedMedia.ThumbnailShowPlayIcon = false;
          }

          // Set the play icon for the currently selected video
          value.ThumbnailShowPlayIcon = true;

          // Update the selected thumbnail and carousel position
          SelectedThumbnail = value.Thumbnail;
          CarouselPosition = Medias.IndexOf(value);

          // Update the reference to the newly selected video
          _previouslySelectedMedia = value;
        }
        finally
        {
          _isUpdatingMedia = false; // Allow updates again
        }

        // Manually raise property changed event for SelectedVideo to ensure description update
        OnPropertyChanged(nameof(SelectedMedia));
      }
    }
    private async Task AddNewMedia ()
    {
      // Check if the storage permission is granted
      var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

      // If permission is not granted, request it
      if (status != PermissionStatus.Granted)
      {
        status = await Permissions.RequestAsync<Permissions.StorageRead>();
      }

      // Now check again if the permission is granted
      if (status == PermissionStatus.Granted)
      {
        try
        {
          // Allow the user to pick a video or image file from the media library
          var result = await FilePicker.PickAsync(new PickOptions
          {
            PickerTitle = "Select a video or image file",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.movie", "public.image" } },
                    { DevicePlatform.Android, new[] { "video/*", "image/*" } },
                    { DevicePlatform.UWP, new[] { ".mp4", ".jpg", ".png" } }
                })
          });

          if (result != null)
          {
            // Create a new Video object and add it to the Videos collection
            var media = new Media
            {
              Source = result.FullPath, // Full path of the selected video/image
              Thumbnail = result.FullPath, // Use the same path for thumbnail (or change as needed)
              Description = "" // Customize as needed
            };

            // Add new media to the collection
            Medias.Add(media);
            OnThumbnailTapped(media); // Select the newly added media
          }
        }
        catch (Exception ex)
        {
          // Handle exceptions (e.g., user canceled the operation)
          await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
      }
      else
      {
        // Handle permission denial
        await Application.Current.MainPage.DisplayAlert("Permission Denied", "Cannot access media files. Please check app permissions in settings.", "OK");
      }
    }

    private void DeleteMedia (Media media)
    {
      if (Medias.Contains(media))
      {
        Medias.Remove(media);
      }
    }

    partial void OnCarouselPositionChanged (int value)
    {
      if (value >= 0 && value < Medias.Count)
      {
        // Update the selected video and thumbnail when carousel position changes
        SelectedMedia = Medias[value];
        SelectedThumbnail = SelectedMedia.Thumbnail;
      }
    }

    private void OnThumbnailTapped (Media tappedMedia)
    {
      // Only update if a new video is tapped and we aren't in the middle of an update
      if (tappedMedia != null && tappedMedia != SelectedMedia && !_isUpdatingMedia)
      {
        try
        {
          _isUpdatingMedia = true; // Prevent recursive updates

          // Update the selected video and carousel position
          CarouselPosition = Medias.IndexOf(tappedMedia);

          // Update SelectedVideo after ensuring the position is set
          SetSelectedMedia(tappedMedia);
        }
        finally
        {
          _isUpdatingMedia = false; // Allow updates again
        }
      }
    }

    private void SetSelectedMedia (Media newMedia)
    {
      if (newMedia != null && !_isUpdatingMedia)
      {
        // Prevent re-entrant updates
        _isUpdatingMedia = true;

        try
        {
          selectedMedia = newMedia;

          // Raise the property changed event to ensure description updates
          OnPropertyChanged(nameof(SelectedMedia));
          OnPropertyChanged(nameof(SelectedMedia.Description));
        }
        finally
        {
          _isUpdatingMedia = false;
        }
      }
    }

    private void OnNavigateLeft ()
    {
      try
      {
        _isUpdatingMedia = true;
        if (CarouselPosition > 0)
        {
          CarouselPosition--;
          SelectedMedia = Medias[CarouselPosition];
        }
        else
        {
          // If the video is first in the list, navigate to the last video
          CarouselPosition = Medias.Count - 1;
          SelectedMedia = Medias[CarouselPosition];
        }
      }
      finally
      {
        _isUpdatingMedia = false;
      }
    }

    private void OnNavigateRight ()
    {
      try
      {
        _isUpdatingMedia = true;
        if (CarouselPosition < Medias.Count - 1)
        {
          CarouselPosition++;
          SelectedMedia = Medias[CarouselPosition];
        }
        else
        {
          // If the video is last in the list, navigate to the first video
          CarouselPosition = 0;
          SelectedMedia = Medias[CarouselPosition];
        }
      }
      finally
      {
        _isUpdatingMedia = false;
      }
    }
  }
}
