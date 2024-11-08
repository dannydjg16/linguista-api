using linguista_api.Models.Completions;

namespace linguista_api.Repositories.Interfaces
{
	public interface ICompletionsRepository
	{
        public Task<CompletionsResponse> SendCompletionRequest(CompletionsRequest request);
    }
}
