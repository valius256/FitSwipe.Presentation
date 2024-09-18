using System.Text.Json.Serialization;

namespace FitSwipe.Shared.Dtos
{
    public class AuthenResponseDto
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
