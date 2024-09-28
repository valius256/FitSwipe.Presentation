using FitSwipe.Shared.Enums;
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Tags
{
    public class GetTagDto : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TagType TagType { get; set; }
        public string? TagColor { get; set; }
        public string CreateById { get; set; } = string.Empty;
        public string? TagImage { get; set; }
        public SpecialTag? SpecialTag { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public RecordStatus RecordStatus { get; set; }
        public int DisplaySize { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
