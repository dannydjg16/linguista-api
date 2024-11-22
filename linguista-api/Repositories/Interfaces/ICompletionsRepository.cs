using linguista_api.Models.Completions;

namespace linguista_api.Repositories.Interfaces
{
	public interface ICompletionsRepository
	{
        Task<CompletionsResponse?> SendCompletionRequest(CompletionsRequest request);
        Task<Stream?> GenerateTextToSpeech(TtsRequest request);
    }
}
