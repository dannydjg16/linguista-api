using Newtonsoft.Json;

namespace linguista_api.Models.Completions
{
	public class CompletionsRequest
	{
        [JsonProperty("model")]
        public string? ChatModel { get; set; }
        [JsonProperty("messages")]
        public List<Message>? Messages { get; set; }
        [JsonProperty("max_completion_tokens")]
        public int MaxTokens { get; set; }
        [JsonProperty("top_p")]
        public int TopP { get; set; }
    }
}