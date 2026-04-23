using linguista_api.Models.Completions;
using linguista_api.Models.Image;

namespace linguista_api.Repositories.Interfaces
{
	public interface ICompletionsRepository
	{
        Task<CompletionsResponse?> SendCompletionRequest(CompletionsRequest request);
        Task<Stream?> GenerateTextToSpeech(TtsRequest request);
        Task<byte[]> GenerateImage(ImageGenerationRequest request);
    }
}