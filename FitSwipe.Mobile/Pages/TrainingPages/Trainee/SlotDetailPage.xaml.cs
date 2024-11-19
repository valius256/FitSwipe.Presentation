using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using FitSwipe.Mobile.Pages.PayingPages;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class SlotDetailPage : ContentPage
{
    public ObservableCollection<GetSlotVideoDto> _videos { get; set; } = [];
    private bool _isSwipe = true;
    private GetSlotDetailDto _slotDetail = new();
    private Guid _slotId;

    public ObservableCollection<GetSlotVideoDto> Videos
    {
        get => _videos;
        set
        {
            _videos = value;
            OnPropertyChanged(nameof(Videos)); // Notify UI of the change
        }
    }
    public GetSlotDetailDto SlotDetail
    {
        get => _slotDetail;
        set
        {
            _slotDetail = value;
            OnPropertyChanged(nameof(SlotDetail)); // Notify UI of the change
        }
    }
    public SlotDetailPage(Guid slotId)
	{
		InitializeComponent();    
        _slotId = slotId;
        Setup();
    }
    private async void Setup()
    {   
        await FetchData();
    }
    private async Task FetchData()
    {
        loadingDialog.IsVisible = true;
        pageContent.IsVisible = false;
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (token == null)
            {
                await Shell.Current.GoToAsync("//SignIn");
                throw new Exception("Lỗi xác thực. Vui lòng đăng nhập lại");
            }
            var result = await Fetcher.GetAsync<GetSlotDetailDto>($"api/Slot/get-slot-by-id?slotId={_slotId}", token);
            if (result != null)
            {
                SlotDetail = result;
                Videos = SlotDetail.Videos.ToObservableCollection();
                if (SlotDetail.Videos.Count > 0)
                {
                    ChangedVideoSelected(0);
                }
            }
            BindingContext = this;

        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", "Có lỗi xảy ra .Err : " + ex.Message, "OK");
        }
        loadingDialog.IsVisible = false;
        pageContent.IsVisible = true;

    }

    private void rightArrow_Clicked(object sender, EventArgs e)
    {
        int currentPosition = videoCarousel.Position;

        if (currentPosition < videoCarousel.ItemsSource.Cast<object>().Count() - 1)
        {
            videoCarousel.Position = currentPosition + 1;
        }
        ChangedVideoSelected(videoCarousel.Position);
    }

    private void leftArrow_Clicked(object sender, EventArgs e)
    {
        int currentPosition = videoCarousel.Position;
        int maxPosition = videoCarousel.ItemsSource.Cast<object>().Count() - 1;
        if (currentPosition > 0)
        {
            videoCarousel.Position = currentPosition - 1;
        }
        ChangedVideoSelected(videoCarousel.Position);
    }

    private void thumbnailTap_Tapped(object sender, TappedEventArgs e)
    {
        var frame = (Frame)sender;
        var tap = frame.GestureRecognizers[0] as TapGestureRecognizer;
        if (tap != null)
        {
            var boundedItem = tap?.CommandParameter as GetSlotVideoDto;
            if (boundedItem != null)
            {
                var video = SlotDetail.Videos.FirstOrDefault(ts => ts.Id == boundedItem.Id);
                if (video != null)
                {
                    ChangedVideoSelected(SlotDetail.Videos.IndexOf(video));
                }
            }
        }

    }

    private async void ChangedVideoSelected(int index)
    {
        if (index >= 0)
        {
            _isSwipe = false;
            // Play or prepare the video at the new position
            //PlayMediaElementAtPosition(index);

            videoCarousel.Position = index;
            for (int i = 0; i < SlotDetail.Videos.Count; i++)
            {
                SlotDetail.Videos[i].ThumbnailShowPlayIcon = index == i;
            }
            lblDescription.Text = Videos[index].Description;
            //SetMediaElementSource(Videos[index].VideoSource);

            await Task.Delay(500);
            _isSwipe = true;
        }
    }

    private void MediaElement_StateChanged(object sender, MediaStateChangedEventArgs e)
    {
        var mediaElement = sender as MediaElement;
        if (mediaElement != null)
        {
            var loadingIndicator = (ActivityIndicator)((Grid)mediaElement.Parent).Children[1];

            // Show the loading indicator if the video is loading
            if (mediaElement.CurrentState == MediaElementState.Opening)
            {
                loadingIndicator.IsVisible = true;
                loadingIndicator.IsRunning = true;
            }
            else
            {
                // Hide the loading indicator once the video is ready to play
                loadingIndicator.IsVisible = false;
                loadingIndicator.IsRunning = false;
            }
        }
        
    }
    private void videoCarousel_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        if (_isSwipe)
        {
            int newPosition = videoCarousel.Position;

            var currentItem = videoCarousel.CurrentItem;

            var views = videoCarousel.VisibleViews.ToList();
            foreach (var visibleView in views)
            {
                var video = visibleView.FindByName<MediaElement>("videoPlayer");
                // We found the view that corresponds to the current item
                if (video != null)
                {
                    if (visibleView.BindingContext != currentItem)
                    {
                        video.Stop();
                    }
                }
            }
            var boundedItem = currentItem as GetSlotVideoDto;
            if (boundedItem != null)
            {
                var video = SlotDetail.Videos.FirstOrDefault(ts => ts.Id == boundedItem.Id);
                if (video != null)
                {
                    ChangedVideoSelected(SlotDetail.Videos.IndexOf(video));
                }
            }
            lblDescription.Text = Videos[newPosition].Description;
            //SetMediaElementSource(Videos[newPosition].VideoSource);
        }

    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void btnPay_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new PayingCheck(new List<GetSlotDetailDto> { _slotDetail }, FetchData));
    }
}