using linguista_api.Models.Completions;
using linguista_api.Models.TTS;

namespace linguista_api.Services.Interfaces
{
	public interface ICompletionsService
	{
        Task<CompletionsResponse?> PrepareRequestReturnObject(CompletionsRequest request);
        Task<Stream?> GenerateTextToSpeech(TtsRequest request);
        Task<CompletionsResponseWithTtsData?> GetCompletionAndTts(CompletionsRequestWithTts request);
    }
}