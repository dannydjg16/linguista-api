using linguista_api.Models.Completions;

namespace linguista_api.Models.TTS
{
	public class CompletionsResponseWithTtsData : CompletionsResponse
	{
		public Stream? TtsStream { get; set;}
	}
}