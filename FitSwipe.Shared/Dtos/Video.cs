using System.ComponentModel;

namespace FitSwipe.Shared.Dtos
{
  public class Video : INotifyPropertyChanged
  {
    private bool _thumbnailShowPlayIcon;

    public string VideoSource { get; set; } = string.Empty;
    public string ThumbnailSource { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; 
    public bool IsVideo => !string.IsNullOrEmpty(VideoSource); // Determine if it is a video

    public bool thumbnailShowPlayIcon
    {
      get => _thumbnailShowPlayIcon;
      set
      {
        if (_thumbnailShowPlayIcon != value)
        {
          _thumbnailShowPlayIcon = value;
          OnPropertyChanged(nameof(thumbnailShowPlayIcon));
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged (string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
