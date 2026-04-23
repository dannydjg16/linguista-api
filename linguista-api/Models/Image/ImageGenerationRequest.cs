using Newtonsoft.Json;

namespace linguista_api.Models.Image
{
	public class ImageGenerationRequest
	{
        [JsonProperty("model")]
        public string? Model { get; set; }
        [JsonProperty("prompt")]
        public string? Prompt { get; set; }
        [JsonProperty("size")]
        public string? Size { get; set; }
    }
}

