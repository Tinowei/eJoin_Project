using System.Text.Json.Serialization;

namespace Admin.Models.Settings
{
    public class JwtSettings
    {
        public const string SettingKey = "JwtSettings";

        [JsonPropertyName("Issuer")]
        public string Issuer { get; set; }
        [JsonPropertyName("SignKey")]
        public string SignKey { get; set; }
    }
}
