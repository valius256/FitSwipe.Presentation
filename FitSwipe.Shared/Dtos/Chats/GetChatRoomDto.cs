using FitSwipe.Shared.Dtos.Users;

namespace FitSwipe.Shared.Dtos.Chats
{
    public class GetChatRoomDto
    {

        public string UserFirebaseId { get; set; } = string.Empty;
        public Guid ChatRoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public GetUserDto User { get; set; } = new GetUserDto();
    }
}
