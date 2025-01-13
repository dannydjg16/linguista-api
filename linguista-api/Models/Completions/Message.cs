using Newtonsoft.Json;

namespace linguista_api.Models.Completions
{
	public class Message
	{
        [JsonProperty("role")]
        public string? Role { get; set; }
        [JsonProperty("content")]
        public string? Content { get; set; }
        [JsonProperty("additionalContent")]
        public string? AdditionalContent { get; set; }
    }
}