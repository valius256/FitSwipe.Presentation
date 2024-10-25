
using FitSwipe.Shared.Dtos;
using FitSwipe.Shared.Dtos.Chats;
using FitSwipe.Shared.Dtos.Users;

namespace FitSwipe.Shared.Utils
{
    public static class Shortcut
    {

        public static async Task<GetUserDto?> GetLoginedUser(string token)
        {
            return await Fetcher.GetAsync<GetUserDto>("api/authentication/who-am-i", token);
        }

        public static async Task<GetChatRoomIdDto?> CreateChatRoomSolo(string token, string toUserId)
        {
            var result = await Fetcher.PostAsync<BlankDto, GetChatRoomIdDto>($"api/RealtimeChat/chat1-1?toUserFirebaseId={toUserId}",new BlankDto(), token);
            return result;

        }
    }
}
