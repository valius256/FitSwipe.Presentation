
namespace FitSwipe.Shared.Dtos.Chats
{
    public class GetMessageDto
    {
        public string UserFirebaseId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        //UI Statuses
        public string BorderColor { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public string TextColor { get; set; } = string.Empty;
        public string Margin { get; set; } = string.Empty;
        public string LeftAvatarSource { get; set; } = string.Empty;
        public string RightAvatarSource { get; set; } = string.Empty;
        public string AvatarColor { get; set; } = string.Empty;
    }
}
