
using FitSwipe.Shared.Dtos.Users;

namespace FitSwipe.Shared.Utils
{
    public static class Shortcut
    {

        public static async Task<GetUserDto?> GetLoginedUser(string token)
        {
            return await Fetcher.GetAsync<GetUserDto>("api/authentication/who-am-i", token);
        }
    }
}
