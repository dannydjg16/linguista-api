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

		public async Task<string> PrepareRequest(CompletionsRequest request)
		{
			var response = await _completionsRepository.SendCompletionRequest(request);

			if (response.Choices != null && response.Choices[0].Message != null)
			{
				var completionString = response?.Choices[0]?.Message?.Content;

				//DoThingsWithRequestAndResponse(request, response);

				return completionString;
            }

			return null;
		}

		public async Task DoThingsWithRequestAndResponse(CompletionsRequest request, CompletionsResponse response)
		{
			// Do things like save to database, track user token amount, etc etc
		}
	}
}