using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using linguista_api.Globals;
using linguista_api.Models.Completions;
using linguista_api.Models.Image;
using linguista_api.Repositories.Interfaces;
using Newtonsoft.Json;

namespace linguista_api.Repositories
{
	public class CompletionsRepository: ICompletionsRepository
	{
        private static readonly HttpClient client = new HttpClient();
        private readonly string _openAiKey;

        public CompletionsRepository(IConfiguration configuration)
		{
            _openAiKey = configuration.GetValue<string>(Constants.OpenAiKey);
		}

        public async Task<CompletionsResponse?> SendCompletionRequest(CompletionsRequest request)
		{
            var url = Constants.OpenAiBaseUrl + Constants.CompletionsEndpoint;
            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiKey);

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            var responseObject = JsonConvert.DeserializeObject<CompletionsResponse>(responseString);

            return responseObject;
        }

        public async Task<Stream?> GenerateTextToSpeech(TtsRequest request)
        {
            var url = Constants.OpenAiBaseUrl + Constants.AudioSpeechEndpoint;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiKey);

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

            // If response isnt success, error out. This may be better to update with try/catch for retries
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(error);
            }

            var responseStream = await response.Content.ReadAsStreamAsync();

            Console.WriteLine(responseStream);

            return responseStream;
        }

        public async Task<byte[]> GenerateImage(ImageGenerationRequest request)
        {
            var url = Constants.OpenAiBaseUrl + Constants.ImageGenerationEndpoint;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiKey);

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            using var doc = JsonDocument.Parse(responseString);
            var base64 = doc.RootElement
                            .GetProperty("data")[0]
                            .GetProperty("b64_json")
                            .GetString();

            return Convert.FromBase64String(base64);
        }
    }
}