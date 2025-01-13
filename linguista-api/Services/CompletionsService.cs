using linguista_api.Models.Completions;
using linguista_api.Repositories.Interfaces;
using linguista_api.Services.Interfaces;

namespace linguista_api.Services
{
    public class CompletionsService : ICompletionsService
    {
        public ICompletionsRepository _completionsRepository;

        public CompletionsService(ICompletionsRepository completionsRepository)
        {
            _completionsRepository = completionsRepository;
        }

        public async Task<CompletionsResponse?> PrepareRequestReturnObject(CompletionsRequest request)
        {
            var response = await _completionsRepository.SendCompletionRequest(request);

            if (response?.Choices != null && response.Choices[0].Message != null)
            {
                return response;
            }

            return null;
        }

        //public async Task<string?> TransliterateString(CompletionsResponse response)
        //{
        //    // make the Completions Request

        //    //var response = await

        //    // add string received back to the response somehow. 
        //}

        public async Task<Stream?> GenerateTextToSpeech(TtsRequest request)
        {
            var response = await _completionsRepository.GenerateTextToSpeech(request);

            return response;
        }
    }
}