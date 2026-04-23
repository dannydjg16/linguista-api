using linguista_api.Models.Completions;
using linguista_api.Models.Image;

namespace linguista_api.Services.Interfaces
{
	public interface ICompletionsService
	{
        Task<CompletionsResponse?> PrepareRequestReturnObject(CompletionsRequest request);
        Task<CompletionsResponse?> PrepareRequestAndTransliterate(CompletionsRequest request);
        Task<Stream?> GenerateTextToSpeech(TtsRequest request);
        Task<byte[]> GenerateImage(ImageGenerationRequest request);
    }
}