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

        private CompletionsRequest FilterCompletionsRequest(CompletionsRequest request, int maxLength=int.MaxValue)
        {
            if (request?.Messages?.Count < maxLength)
            {
                return request;
            }

            var systemMessages = request?.Messages?.Where(m => m.Role == "system").ToList();

            var numberOfNonSystemElementsToKeep = maxLength - systemMessages.Count;

            var nonSystemElementsToKeep = request?.Messages?.TakeLast(numberOfNonSystemElementsToKeep);

            var trimmedList = systemMessages.Concat(nonSystemElementsToKeep).ToList();

            request.Messages = trimmedList;

            return request;
        }

        public async Task<CompletionsResponse?> PrepareRequestAndTransliterate(CompletionsRequest request)
        {
            var response = await _completionsRepository.SendCompletionRequest(request);

            if (response?.Choices != null && response.Choices[0].Message != null)
            {
                var transliterationRequest = new CompletionsRequest
                {
                    ChatModel = "gpt-3.5-turbo",
                    MaxTokens = 100,
                    Messages = new List<Message>
                    {
                        new Message
                        {
                            Role = "system",
                            Content = "Transliterate the following Message from farsi to English." 
                        },
                        new Message
                        {
                            Role = "user",
                            Content = response.Choices.Last().Message.Content
                        }

                    },
                    TopP = 1
                };

                var transliterationResponse = await _completionsRepository.SendCompletionRequest(transliterationRequest);

                response.Choices[0].Message.AdditionalContent = transliterationResponse.Choices[0].Message.Content;

                return response;
            }

            return null;
        }

        public async Task<Stream?> GenerateTextToSpeech(TtsRequest request)
        {
            var response = await _completionsRepository.GenerateTextToSpeech(request);

            return response;
        }
    }
}