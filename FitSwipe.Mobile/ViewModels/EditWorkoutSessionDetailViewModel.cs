using CommunityToolkit.Mvvm.ComponentModel;
using FitSwipe.Shared.Dtos;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Media;
using Microsoft.Maui.ApplicationModel;
using CommunityToolkit.Mvvm.Input;
using FitSwipe.Shared.Dtos.Slots;
using CommunityToolkit.Maui.Core.Extensions;
using FitSwipe.Mobile.Controls;
using FitSwipe.Shared.Utils;
using FitSwipe.Mobile.Utils;
using FitSwipe.Shared.Dtos.Medias;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class EditWorkoutSessionDetailViewModel : ObservableObject
  {
    //public ObservableCollection<Media> Medias { get; set; } = new ObservableCollection<Media>();
    public ICommand ChangeCarouselPositionCommand { get; }
    public ICommand NavigateLeftCommand { get; }
    public ICommand NavigateRightCommand { get; }
    public ICommand AddMediaCommand { get; }
    public ICommand DeleteMediaCommand { get; }
    public ICommand ChangeThumbnailCommand { get; }
    public ICommand SaveCommand { get; }

    [ObservableProperty]
    private GetSlotDetailDto slotDetail = new GetSlotDetailDto();
    [ObservableProperty]
    private ObservableCollection<GetSlotVideoDto> slotVideos = new ObservableCollection<GetSlotVideoDto>();
    [ObservableProperty]
    private string selectedThumbnail = string.Empty;
    [ObservableProperty]
    private GetSlotVideoDto selectedSlotVideo = new();

    [ObservableProperty]
    private int carouselPosition;

    public bool IsChangedFlag = false;
    [ObservableProperty]
    private ObservableCollection<string> _newVideos = new ObservableCollection<string>();
    private LoadingDialog _loadingDialog;
    private ScrollView _pageContent;
    private Guid _slotId;
    private int? _slotNumber;
    //private Func<Task> _refreshData;

    public EditWorkoutSessionDetailViewModel (Guid slotId, LoadingDialog loadingDialog, ScrollView pageContent, int? slotNumber = null)
    {
      _loadingDialog = loadingDialog;
      _slotId = slotId;
      _pageContent = pageContent;
      _slotNumber = slotNumber;

      ChangeCarouselPositionCommand = new Command<GetSlotVideoDto>(OnThumbnailTapped);
      NavigateLeftCommand = new Command(OnNavigateLeft);
      NavigateRightCommand = new Command(OnNavigateRight);
      SaveCommand = new Command(async () => await Save());
      AddMediaCommand = new Command(async () => await AddNewMedia());
      DeleteMediaCommand = new Command<GetSlotVideoDto>(DeleteMedia);
      ChangeThumbnailCommand = new RelayCommand<GetSlotVideoDto>(OnThumbnailTapped);
      FetchSlotDetail();
      
    }
    private async void FetchSlotDetail()
    {
        _pageContent.IsVisible = false;
        _loadingDialog.IsVisible = true;
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token == null)
            {
                throw new Exception("Lỗi xác thực, vui lòng đăng nhập lại");
            }
            var result = await Fetcher.GetAsync<GetSlotDetailDto>($"api/Slot/get-slot-by-id?slotId={_slotId}",token);
            if (result != null)
            {
                if (_slotNumber != null) result.SlotNumber = _slotNumber;
                SlotDetail = result;
                // Initialize selected video
                if (SlotDetail.Videos.Count > 0)
                {
                    SelectedSlotVideo = SlotDetail.Videos[0];
                    CarouselPosition = 0;
                    SelectedThumbnail = SlotDetail.Videos[0].ThumbnailUrl;
                    SlotDetail.Videos[0].ThumbnailShowPlayIcon = true; // Show play icon for the first video
                }
                SlotVideos = SlotDetail.Videos.ToObservableCollection();
            }
            
        } catch (Exception ex) 
        {
            if (Application.Current != null && Application.Current.MainPage != null)
            {
                // Handle exceptions (e.g., user canceled the operation)
                await Application.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
            }
        }
        _loadingDialog.IsVisible = false;
        _pageContent.IsVisible = true;
    }
    private async Task AddNewMedia ()
    {
            if (!_loadingDialog.IsVisible)
            {

                try
                {
                    // Allow the user to pick a video or image file from the media library
                    var result = await MediaPicker.PickVideoAsync();
                    if (result != null)
                    {
                        var fileInfo = new FileInfo(result.FullPath);

                        // Get the file size in bytes
                        long fileSizeInBytes = fileInfo.Length;

                        // Convert to megabytes (optional)
                        double fileSizeInMB = fileSizeInBytes / (1024.0 * 1024.0);

                        // Check if file size exceeds 25 MB
                        if (fileSizeInMB > 25)
                        {
                            throw new Exception("Video đã vượt quá 25MB. Vui lòng chọn video khác nhỏ hơn");
                        }
                        if (NewVideos.Contains(result.FullPath))
                        {
                            throw new Exception("Bạn đã đăng file này");
                        }
                        // Create a new Video object and add it to the Videos collection
                        var media = new GetSlotVideoDto
                        {
                            VideoUrl = result.FullPath, // Full path of the selected video/image
                            ThumbnailUrl = result.FullPath, // Use the same path for thumbnail (or change as needed)
                            Description = "",
                            ThumbnailShowPlayIcon = false,
                            IsFromFilePath = true
                        };

                        // Add new media to the collection
                        NewVideos.Add(result.FullPath);
                        SlotDetail.Videos.Add(media);
                        SlotVideos.Add(media);

                        OnThumbnailTapped(media); // Select the newly added media
                    }
                }
                catch (Exception ex)
                {
                    if (Application.Current != null && Application.Current.MainPage != null)
                    {
                        // Handle exceptions (e.g., user canceled the operation)
                        await Application.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
                    }
                }
                
                //else
                //{
                //    if (Application.Current != null && Application.Current.MainPage != null)
                //    {
                //        await Application.Current.MainPage.DisplayAlert("Quyền truy cập bị từ chối", "Không thể truy cập các file phương tiện. Vui lòng cho phép trong phần cài đặt", "OK");

                //    }
                //}
            }
    }

    private void DeleteMedia (GetSlotVideoDto media)
    {
            if (SlotDetail.Videos.Contains(media))
            {
                var pos = SlotDetail.Videos.IndexOf(media);
                var newPos = pos - 1;
                if (newPos < 0 && SlotDetail.Videos.Count > 1)
                {
                    newPos = SlotDetail.Videos.Count - 1;
                }
                if (newPos >= 0)
                {
                    CarouselPosition = newPos;
                    SetSelectedMedia(SlotDetail.Videos[CarouselPosition]);
                }
                if (media.IsFromFilePath)
                {
                    var pathToRemove = _newVideos.FirstOrDefault(sv => sv == media.VideoUrl);
                    if (pathToRemove != null)
                    {
                        
                        NewVideos.Remove(pathToRemove);
                        
                    }
                }
                SlotDetail.Videos.Remove(media);
                SlotVideos.Remove(media);
            }
    }

    //partial void OnCarouselPositionChanged (int value)
    //{
    //  if (value >= 0 && value < SlotDetail.Videos.Count)
    //  {
    //    // Update the selected video and thumbnail when carousel position changes
    //    SelectedSlotVideo = SlotDetail.Videos[value];
    //    SelectedThumbnail = SelectedSlotVideo.ThumbnailUrl;
    //  }
    //}

    private void OnThumbnailTapped (GetSlotVideoDto? tappedMedia)
    {
      // Only update if a new video is tapped and we aren't in the middle of an update
      if (tappedMedia != null && tappedMedia != SelectedSlotVideo)
      {
          // Update the selected video and carousel position
          CarouselPosition = Math.Max(0, SlotDetail.Videos.IndexOf(tappedMedia));

          // Update SelectedVideo after ensuring the position is set
          SetSelectedMedia(tappedMedia);
          //_isUpdatingMedia = false; // Allow updates again        
      }
    }

    private void SetSelectedMedia (GetSlotVideoDto newMedia)
    {
      if (newMedia != null)
      {
        SelectedSlotVideo = newMedia;
        foreach (var item in SlotDetail.Videos) 
        { 
            item.ThumbnailShowPlayIcon = (item.Id == newMedia.Id);
            SlotVideos[SlotDetail.Videos.IndexOf(item)].ThumbnailShowPlayIcon = item.ThumbnailShowPlayIcon;
        }
      }
    }

    private void OnNavigateLeft ()
    {

        if (CarouselPosition > 0)
        {
            CarouselPosition--;
        }
        //DO NOT LOoP DUE TO POOR PERFORMANCE ISSUE
        //else
        //{
        //    // If the video is first in the list, navigate to the last video
        //    CarouselPosition = SlotDetail.Videos.Count - 1;       
        //}
        SetSelectedMedia(SlotDetail.Videos[CarouselPosition]);

    }

    private void OnNavigateRight ()
    {
        if (CarouselPosition < SlotDetail.Videos.Count - 1)
        {
          CarouselPosition++;
        }
        //DO NOT LOoP DUE TO POOR PERFORMANCE ISSUE
        //else
        //{
        //  // If the video is last in the list, navigate to the first video
        //  CarouselPosition = 0;
        //}     
        SetSelectedMedia(SlotDetail.Videos[CarouselPosition]);
    }

        private async Task Save()
        {
            if (Application.Current != null && Application.Current.MainPage != null && !_loadingDialog.IsVisible)
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Lưu thay đổi", "Bạn có chắc chắn về hành động này không?", "Có","Không");
                if (answer)
                {
                    _loadingDialog.IsVisible = true;
                    try
                    {
                        var token = await SecureStorage.GetAsync("auth_token");
                        if (token == null)
                        {
                            throw new Exception("Lỗi xác thực, vui lòng đăng nhập lại");
                        }
                        var requestVideos = new List<RequestCreateSlotVideoDto>();
                        var oldVideos = SlotDetail.Videos.Where(sv => !sv.IsFromFilePath).ToList();
                        foreach (var videos in oldVideos)
                        {
                            requestVideos.Add(new RequestCreateSlotVideoDto
                            {
                                SlotId = _slotId,
                                Description = videos.Description,
                                ThumbnailUrl = videos.ThumbnailUrl,
                                VideoUrl = videos.VideoUrl
                            });
                        }
                        foreach (var filePath in NewVideos)
                        {
                            var videoFromTotalList = SlotDetail.Videos.FirstOrDefault(sv => sv.VideoUrl == filePath);
                            if (videoFromTotalList != null)
                            {
                                var videoResult = await MauiUtils.UploadVideoAsync(new FileResult(filePath), token);
                                if (videoResult == null)
                                {
                                    throw new Exception("Tải video thất bại");
                                }                      
                                requestVideos.Add(new RequestCreateSlotVideoDto
                                {
                                    SlotId = _slotId,
                                    Description = videoFromTotalList?.Description,
                                    ThumbnailUrl = videoResult.FileUrl,
                                    VideoUrl = videoResult.FileUrl
                                });
                            }
                            
                        }
                        var request = new RequestUpdateSlotDetailDto
                        {
                            SlotId = _slotId,
                            SlotVideos = requestVideos,
                            Location = SlotDetail.Location,
                        };
                        await Fetcher.PostAsync("api/Slot/update-slot-detail", request, token);
                        await Application.Current.MainPage.DisplayAlert("Thành công", "Đã lưu thay đổi", "OK");
                        IsChangedFlag = true;
                    } 
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Lỗi", "Có lỗi xãy ra : " + ex.Message, "Có");
                    }
                    _loadingDialog.IsVisible = false;
                }
            }
        }
    }

}
