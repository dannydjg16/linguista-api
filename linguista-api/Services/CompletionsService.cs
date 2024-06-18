using System;
using linguista_api.Models.Completions;
using linguista_api.Repositories.Interfaces;
using linguista_api.Services.Interfaces;

namespace linguista_api.Services
{
	public class CompletionsService: ICompletionsService
	{
		public ICompletionsRepository _completionsRepository;

		public CompletionsService(ICompletionsRepository completionsRepository)
		{
			_completionsRepository = completionsRepository;
		}

		public async Task<CompletionsResponse> PrepareRequest()
		{
			var response = await _completionsRepository.SendCompletionRequest();

			return response;
		}
	}
}