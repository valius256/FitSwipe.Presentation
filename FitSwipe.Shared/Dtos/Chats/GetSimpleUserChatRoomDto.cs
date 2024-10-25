using FitSwipe.Shared.Enums;
using System.ComponentModel;
namespace FitSwipe.Shared.Dtos.Chats
{
    public class GetSimpleUserChatRoomDto : INotifyPropertyChanged
    {
        public string UserFirebaseId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; } = string.Empty;
        public Role Role { get; set; }
        public string? Job { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? SubscriptionLevel { get; set; }
        public PaymentStatus? SubscriptionPaymentStatus { get; set; }

        private int _unseenMessaged;
        public int UnseenMessaged
        {
            get => _unseenMessaged;
            set
            {
                if (_unseenMessaged != value)
                {
                    _unseenMessaged = value;
                    OnPropertyChanged(nameof(UnseenMessaged));
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
