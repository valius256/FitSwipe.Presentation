
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Slots
{
    public class GetSlotVideoDto : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public Guid SlotId { get; set; }
        public string VideoUrl { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsFromFilePath { get; set; } = false;
        private bool _thumbnailShowPlayIcon = false;
        public bool ThumbnailShowPlayIcon
        {
            get => _thumbnailShowPlayIcon;
            set
            {
                _thumbnailShowPlayIcon = value;
                OnPropertyChanged(nameof(ThumbnailShowPlayIcon));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
