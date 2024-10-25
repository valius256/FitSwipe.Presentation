using FitSwipe.Shared.Dtos.Users;
using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Chats
{
    public class GetChatRoomDto : INotifyPropertyChanged
    {        
        public string UserFirebaseId { get; set; } = string.Empty;
        public Guid ChatRoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        private DateTime? _updateDate {  get; set; }
        public DateTime? UpdatedDate
        {
            get => _updateDate;
            set
            {
                _updateDate = value;
                OnPropertyChanged(nameof(UpdatedDate));
            }
        }
        public List<GetSimpleUserChatRoomDto> Users { get; set; } = [];

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
