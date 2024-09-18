using System.Text.Json.Serialization;

namespace FitSwipe.Shared.Models
{
    public class ResponseRoleModel
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}
