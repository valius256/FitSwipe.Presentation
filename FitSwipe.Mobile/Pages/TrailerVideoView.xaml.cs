using CommunityToolkit.Maui.Core.Primitives;

namespace FitSwipe.Mobile.Pages;

public partial class TrailerVideoView : ContentPage
{
    public TrailerVideoView ()
    {
        InitializeComponent();
    }

    private void videoPlaybackButton_Clicked (object sender, EventArgs e)
    {
        if (videoPlayer.CurrentState == MediaElementState.Playing)
        {
            // If the video is playing, pause it and change the button text
            videoPlayer.Pause();
            videoPlaybackButton.Source = "play.png";
        }
        else if (videoPlayer.CurrentState == MediaElementState.Paused || videoPlayer.CurrentState == MediaElementState.Stopped)
        {
            // If the video is paused or stopped, play it and change the button text
            videoPlayer.Play();
            videoPlaybackButton.Source = "pause.png"; 
        }

    }
}