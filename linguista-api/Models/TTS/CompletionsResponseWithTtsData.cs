using linguista_api.Models.Completions;
using Newtonsoft.Json;

namespace linguista_api.Models.TTS
{
	public class CompletionsResponseWithTtsData
	{
        [JsonProperty("CompletionsResponse")]
        public CompletionsResponse? CompletionsResponse { get; set;}
		[JsonProperty("TtsStream")]
		public Stream? TtsStream { get; set;}
        [JsonProperty("SpeechData")]
        public byte[]? SpeechData { get; set; }
        [JsonProperty("ContentType")]
        public string? ContentType { get; set; }
	}
}