using Newtonsoft.Json;

namespace linguista_api.Models.Completions
{
	public class CompletionsResponse
	{
        [JsonProperty("id")]
        public string? Id { get; set; }
        [JsonProperty("object")]
        public string? Object { get; set; }
        [JsonProperty("created")]
        public int Created { get; set; }
        [JsonProperty("model")]
        public string? Model { get; set; }
        [JsonProperty("choices")]
        public List<Choice>? Choices { get; set; }
        [JsonProperty("usage")]
        public Usage? Usage { get; set; }
        [JsonProperty("system_fingerprint")]
        public string? SystemFingerprint { get; set; }
    }

    public class Choice
    {
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("message")]
        public Message Message { get; set; }
        [JsonProperty("logprobs")]
        public string? LogProbs { get; set; }
        [JsonProperty("finish_reason")]
        public string? FinishReason { get; set; }
    }

    public class Usage
    {
        [JsonProperty("prompt_tokens")]
        public int PromptTokens { get; set; }
        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; set; }
        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }
    }
}