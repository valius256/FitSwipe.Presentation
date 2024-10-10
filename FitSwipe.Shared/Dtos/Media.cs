using FitSwipe.Shared.Dtos.Trainings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Shared.Dtos
{
  public class Media : INotifyPropertyChanged
  {
    private bool _thumbnailShowPlayIcon;

    public string Source { get; set; } = string.Empty;
    public string Thumbnail { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Determines if the media is a video based on the Source property.
    public bool IsVideo => !string.IsNullOrEmpty(Source) && Source.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase);

    public bool ThumbnailShowPlayIcon // Capitalize property name for public visibility convention
    {
      get => _thumbnailShowPlayIcon;
      set
      {
        if (_thumbnailShowPlayIcon != value)
        {
          _thumbnailShowPlayIcon = value;
          OnPropertyChanged(nameof(ThumbnailShowPlayIcon)); // Use capitalized property name
        }
      }
    }

    // Event for property changes
    public event PropertyChangedEventHandler PropertyChanged;

    // Method to raise the property changed event
    protected void OnPropertyChanged (string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
