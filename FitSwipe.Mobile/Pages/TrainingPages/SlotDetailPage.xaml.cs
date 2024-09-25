using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using FitSwipe.Shared.Dtos;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Syncfusion.Maui.Core;
using System;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.TrainingPages;

public partial class SlotDetailPage : ContentPage
{
    public ObservableCollection<Video> Videos { get; set; }
    public List<string> ThumbnailsSource { get; set; } = new List<string>();
    private bool _isSwipe = true;
    private string _selectedThumbnail = string.Empty;
    public string SelectedThumbnail {
        get => _selectedThumbnail;
        set
        {
            _selectedThumbnail = value;
            OnPropertyChanged(nameof(SelectedThumbnail)); // Notify UI of the change
        }
    }
    private string _selectedVideo = string.Empty;
    public string SelectedVideo
    {
        get => _selectedThumbnail;
        set
        {
            _selectedThumbnail = value;
            OnPropertyChanged(nameof(SelectedThumbnail)); // Notify UI of the change
        }
    }
    public SlotDetailPage()
	{
		InitializeComponent();
        Videos = new ObservableCollection<Video>
        {
            new Video { VideoSource = "", ThumbnailSource = "https://storage.googleapis.com/ondemandtutor-a049e.appspot.com/images/yQm3GTCOQZSsuQ8F1su2nK7GJ1n2/Avartar_of_23_1720372252189" },
            new Video { VideoSource = "https://storage.googleapis.com/fit-swipe-161d7.appspot.com/videos/719ZkC7AKbYDxU3l1dpkxvKqG3H2/354849733_6982381381780275_2790533324940314360_n.mp4", ThumbnailSource = "https://scontent.xx.fbcdn.net/v/t1.15752-9/355040793_259496903327765_2250752017545809869_n.png?_nc_cat=105&ccb=1-7&_nc_sid=0024fc&_nc_eui2=AeFWIdqK8j8eVscLc8mzanixyN2gcYzbhxPI3aBxjNuHE-vg5ogaE1Kei2SDQ56wInUQem5ZpiSepikZAOJVVAyl&_nc_ohc=eWECrjci1f0Q7kNvgFcXl8B&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent.xx&_nc_gid=A3thyrOJRKcOr59pUAg9l6r&oh=03_Q7cD1QH4ODdT7PIOWlfWqMHwhUrVAAYn6RiqRnbdDTXp8acbqw&oe=6719108B",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris. Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris. Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris. Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris.Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris "},
            new Video { VideoSource = "https://storage.googleapis.com/fit-swipe-161d7.appspot.com/videos/719ZkC7AKbYDxU3l1dpkxvKqG3H2/404568321_7252601748188284_6263480793654348234_n.mp4\r\n", ThumbnailSource = "https://scontent.xx.fbcdn.net/v/t1.15752-9/344313296_970403757648062_993748438218919599_n.jpg?_nc_cat=100&ccb=1-7&_nc_sid=0024fc&_nc_eui2=AeHkRpKxreKfZSzIo_do23Rr1WGB3sqI_iHVYYHeyoj-IU7iO9mwltKkRGI9XXR2BKbOLq9CZHZpn8fLvHFUwDxn&_nc_ohc=v-dcW18xGzIQ7kNvgESR0dI&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent.xx&_nc_gid=AF7_elesra83Rqv7boTY3Ef&oh=03_Q7cD1QEr8rHBlmmJYZKqKjoDalFlf6QhgkDO9vnOX8Xkq7s1Jw&oe=671919C9",
                Description = "Et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris "},
            new Video { VideoSource = "https://storage.googleapis.com/fit-swipe-161d7.appspot.com/videos/719ZkC7AKbYDxU3l1dpkxvKqG3H2/9128162c-9bef-4cf2-bfe3-899348f40282.mp4\r\n", ThumbnailSource = "https://scontent.xx.fbcdn.net/v/t1.15752-9/325065650_550780030447078_3236866959588821204_n.jpg?_nc_cat=105&ccb=1-7&_nc_sid=0024fc&_nc_eui2=AeFGRwdD8MGv18RljtNzXSaDngcctmOR5sWeBxy2Y5HmxcyWZNWDOMvr74umEOMug0xd9LnNg3VNXRlqVF1g706y&_nc_ohc=zPYtwpUujEUQ7kNvgH6pfGP&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent.xx&_nc_gid=AM5qTDh3pSmXtszuIAoW3fv&oh=03_Q7cD1QFWu42TtLy3OSDCnnf8WIhVKeRQcqmE0zCqrbNc1Qp92A&oe=6718ECF4",
                Description = "Ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris "},
            new Video { VideoSource = "https://storage.googleapis.com/fit-swipe-161d7.appspot.com/videos/719ZkC7AKbYDxU3l1dpkxvKqG3H2/460148095_8452406541482038_4436287398299328142_n.mp4", ThumbnailSource = "https://scontent.xx.fbcdn.net/v/t1.15752-9/329800893_603226448266385_7134610475018072819_n.jpg?_nc_cat=105&ccb=1-7&_nc_sid=0024fc&_nc_eui2=AeFclrTDBqOhQ7AO52t-3vj-QntOHHa1swVCe04cdrWzBYWUiUPYxz7T_U5smUAIw4I7OY7t8-v-jSS3EN0q0fWw&_nc_ohc=QI0Y8qP2KEEQ7kNvgGB5Qqy&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent.xx&_nc_gid=AH1l386Ktg4jJpNAo4WfZNy&oh=03_Q7cD1QHhygXZ8NxbgSCtYHzSDJC7dJ0NgcVOgcUiCHe-kB9Isw&oe=671905C9",
                Description = "Labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris "},
            new Video { VideoSource = "https://storage.googleapis.com/fit-swipe-161d7.appspot.com/videos/719ZkC7AKbYDxU3l1dpkxvKqG3H2/3cfd7d94-e4cc-4a12-9cda-e61cbd0eae2c.mp4\r\n", ThumbnailSource = "https://scontent.xx.fbcdn.net/v/t1.15752-9/299724469_1221062608670518_8628630366394999237_n.jpg?_nc_cat=101&ccb=1-7&_nc_sid=0024fc&_nc_eui2=AeHtld8oNH4o9FPDu2O1icZd5QBip8Pqs5XlAGKnw-qzlZNZtketzQKKZ7nlukS-yLiCRPfcsFajeCyEWUo0j0YK&_nc_ohc=__qklvPCQJAQ7kNvgHgh9Ey&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent.xx&oh=03_Q7cD1QFQ_VTm36atN3imQFt4dSvPfiGGYhc-MEszxXz5iiP5Rw&oe=6718FD4D",
                Description = "Lorem ipsum dolor sit amet, consectetur aboris "},
            // Add more videos
        };
        ThumbnailsSource = Videos.Select(v => v.ThumbnailSource).ToList();
        ChangedVideoSelected(0);
        BindingContext = this;
    }

    private void rightArrow_Clicked(object sender, EventArgs e)
    {
        int currentPosition = videoCarousel.Position;

        if (currentPosition < videoCarousel.ItemsSource.Cast<object>().Count() - 1)
        {
            videoCarousel.Position = currentPosition + 1;
        }
        else
        {
            videoCarousel.Position = 0;
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
        else
        {
            // Optionally loop back to the last item
            videoCarousel.Position = maxPosition;
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
        SelectedThumbnail = Videos[index].ThumbnailSource;
        lblDescription.Text = Videos[index].Description;
        //SetMediaElementSource(Videos[index].VideoSource);

        await Task.Delay(500);
        _isSwipe = true;
    }

    private void MediaElement_StateChanged(object sender, CommunityToolkit.Maui.Core.Primitives.MediaStateChangedEventArgs e)
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


    //private void SetMediaElementSource(string newSource)
    //{
    //    var carouselContentView = videoCarousel.FindByName<ContentView>("carouselContentView");

    //    if (carouselContentView != null)
    //    {
    //        // Now find the MediaElement in the DataTemplate
    //        var mediaGrid = (Grid)carouselContentView.Content;
    //        var mediaElement = (MediaElement)mediaGrid.Children[0];
    //        if (mediaElement != null)
    //        {
    //            // Set the source for the MediaElement programmatically
    //            mediaElement.Source = new Uri(newSource, UriKind.RelativeOrAbsolute);
    //        }
    //    }
    //}

    //private MediaElement? GetMediaElementAtPosition(int position)
    //{
    //    var currentView = (videoCarousel.View);
    //    if (currentView.Count > 0)
    //    {
    //        var mediaGrid = (Grid)currentView[position];
    //        var mediaElement = (MediaElement) mediaGrid.Children[0];    
    //        return mediaElement;

    //    }

    //    return null;
    //}

    private void videoCarousel_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        if (_isSwipe)
        {
            int newPosition = videoCarousel.Position;

            SelectedThumbnail = Videos[newPosition].ThumbnailSource;
            lblDescription.Text = Videos[newPosition].Description;
            //SetMediaElementSource(Videos[newPosition].VideoSource);
        }

    }
}