using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using FitSwipe.Mobile.Pages.PayingPages;
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Slots;
using FitSwipe.Shared.Enums;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class SlotDetailPage : ContentPage
{
    public ObservableCollection<GetSlotVideoDto> _videos { get; set; } = [];
    public List<string> ThumbnailsSource { get; set; } = new List<string>();
    private bool _isSwipe = true;
    private GetSlotDetailDto _slotDetail = new();
    private Guid _slotId;
    private string _selectedThumbnail = string.Empty;
    public string SelectedThumbnail {
        get => _selectedThumbnail;
        set
        {
            _selectedThumbnail = value;
            OnPropertyChanged(nameof(SelectedThumbnail)); // Notify UI of the change
        }
    }
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
                    ThumbnailsSource = Videos.Select(v => v.ThumbnailUrl).ToList();
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
            var boundedString = tap?.CommandParameter as string;
            if (boundedString != null)
            {
                SelectedThumbnail = boundedString;
            }
            var pos = ThumbnailsSource.FindIndex(ts => ts == boundedString);
            ChangedVideoSelected(pos);

        }

    }

    private async void ChangedVideoSelected(int index)
    {
        _isSwipe = false;
        // Play or prepare the video at the new position
        //PlayMediaElementAtPosition(index);

        videoCarousel.Position = index;
        SelectedThumbnail = Videos[index].ThumbnailUrl;
        lblDescription.Text = Videos[index].Description;
        //SetMediaElementSource(Videos[index].VideoSource);

        await Task.Delay(500);
        _isSwipe = true;
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

            SelectedThumbnail = Videos[newPosition].ThumbnailUrl;
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