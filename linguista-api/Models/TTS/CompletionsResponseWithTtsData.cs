using linguista_api.Models.Completions;
using Newtonsoft.Json;

namespace linguista_api.Models.TTS
{
	public class CompletionsResponseWithTtsData : CompletionsResponse
	{
		[JsonProperty("TtsStream")]
		public Stream? TtsStream { get; set;}
	}
}