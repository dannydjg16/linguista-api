using Newtonsoft.Json;

namespace linguista_api.Models.Completions
{
	public class CompletionsRequest
	{
        [JsonProperty("model")]
        public string? ChatModel { get; set; }
        [JsonProperty("messages")]
        public List<Message>? Messages { get; set; }
        [JsonProperty("temperature")]
        public float Temperature { get; set; }
        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }
        [JsonProperty("top_p")]
        public int TopP { get; set; }
    }
}