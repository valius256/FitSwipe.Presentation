namespace FitSwipe.Mobile.Pages.ChatPages;

public partial class ChatDetail : ContentPage
{
    private string _userFirebaseId;
    private Guid _chatRoomId;
    public ChatDetail(string userFirebaseId, Guid chatRoomId)
    {
        InitializeComponent();
        _userFirebaseId = userFirebaseId;
        _chatRoomId = chatRoomId;
    }
}