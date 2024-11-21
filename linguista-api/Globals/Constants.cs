using System;
namespace linguista_api.Globals
{
	public class Constants
	{
		// Settings Key Values
		public const string OpenAiKey = "OpenAiKey";

		// Urls
		public const string OpenAiBaseUrl = "https://api.openai.com";

        // Endpoints
        public const string CompletionsEndpoint = "/v1/chat/completions";
        public const string AudioSpeechEndpoint = "/v1/audio/speech";
    }
}