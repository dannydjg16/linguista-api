using Newtonsoft.Json;

namespace linguista_api.Models.Completions
{
	public class CompletionsResponse
	{
		public string? Id { get; set; }
        public string? Object { get; set; }
        public int Created { get; set; }
        public string? Model { get; set; }
        public List<Choice>? Choices { get; set; }
        public Usage? Usage { get; set; }
        [JsonProperty("system_fingerprint")]
        public string? SystemFingerprint { get; set; }
    }

    public class Choice
    {
        public int Index { get; set; }
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