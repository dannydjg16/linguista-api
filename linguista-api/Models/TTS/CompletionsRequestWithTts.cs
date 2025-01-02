using linguista_api.Models.Completions;
using Newtonsoft.Json;

namespace linguista_api.Models.TTS
{
	public class CompletionsRequestWithTts: CompletionsRequest
	{
        [JsonProperty("TtsRequest")]
        public TtsRequest? TtsRequest { get; set; }
	}
}