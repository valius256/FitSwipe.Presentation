
using FitSwipe.Shared.Enum;
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Transactions
{
    public class GetTransactionDto : INotifyPropertyChanged
    {
        public string TranscationCode { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int Amount { get; set; }
        public TransactionMethod Method { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<Guid> TransactionSlot { get; set; } = new List<Guid>();
        private string _amountDisplay = string.Empty;
        public string AmountDisplay
        {
            get => _amountDisplay;
            set
            {
                if (_amountDisplay != value)
                {
                    _amountDisplay = value;
                    OnPropertyChanged(nameof(AmountDisplay));
                }
            }
        }
        private string _amountColor = "#000000";
        public string AmountColor
        {
            get => _amountColor;
            set
            {
                if (_amountColor != value)
                {
                    _amountColor = value;
                    OnPropertyChanged(nameof(AmountColor));
                }
            }
        }
        private string _textColor = "#000000";
        public string TextColor
        {
            get => _textColor;
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    OnPropertyChanged(nameof(TextColor));
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
