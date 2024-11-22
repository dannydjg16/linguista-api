using linguista_api.Models.Completions;

namespace linguista_api.Services.Interfaces
{
	public interface ICompletionsService
	{
        Task<CompletionsResponse?> PrepareRequestReturnObject(CompletionsRequest request);
        Task<Stream?> GenerateTextToSpeech(TtsRequest request);
    }
}