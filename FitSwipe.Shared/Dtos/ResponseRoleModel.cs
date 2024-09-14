using System.Text.Json.Serialization;

namespace FitSwipe.Shared.Dtos
{
    public class ResponseRoleModel
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}
