namespace FitSwipe.Shared.Dtos
{
    public class SendMessageParameter
    {
        public string ChatRoomId { get; set; }
        public string UserFirebaseId { get; set; }
        public string Message { get; set; }
        public bool IsGroup { get; set; }
    }
}
