using FitSwipe.Shared.Enums;
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.RequestWithdraw
{
    public class GetRequestWithdrawDto : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public required string UserId { get; set; }
        public string? HandlerId { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string OperatorMessage { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public RequestStatus Status { get; set; }
        private string _statusDisplay = string.Empty;
        public string StatusDisplay
        {
            get => _statusDisplay;
            set
            {
                if (_statusDisplay != value)
                {
                    _statusDisplay = value;
                    OnPropertyChanged(nameof(StatusDisplay));
                }
            }
        }
        private string _statusBackground = "#000000";
        public string StatusBackground
        {
            get => _statusBackground;
            set
            {
                if (_statusBackground != value)
                {
                    _statusBackground = value;
                    OnPropertyChanged(nameof(StatusBackground));
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
