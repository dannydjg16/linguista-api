using linguista_api.Models.Completions;

namespace linguista_api.Services.Interfaces
{
	public interface ICompletionsService
	{
        public Task<CompletionsResponse> PrepareRequestReturnObject(CompletionsRequest request);
    }
}