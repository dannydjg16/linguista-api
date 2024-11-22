using Newtonsoft.Json;

namespace linguista_api.Models.Completions
{
	public class TtsRequest
	{
        [JsonProperty("model")]
        public string? Model { get; set; }
        [JsonProperty("input")]
        public string? Input { get; set; }
        [JsonProperty("voice")]
        public string? Voice { get; set; }
    }
}