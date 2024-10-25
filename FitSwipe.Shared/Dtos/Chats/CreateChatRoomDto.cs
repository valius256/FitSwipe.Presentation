

namespace FitSwipe.Shared.Dtos.Chats
{
    public class CreateChatRoomDto
    {
        public bool IsGroup { get; set; } // True for group chat, False for 1-on-1 chat
        public List<string> UserFirebaseIds { get; set; } = [];
        public string ChatRoomName { get; set; } = string.Empty;
    }
}
