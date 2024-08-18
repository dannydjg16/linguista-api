using System;
using System.Net.Http.Headers;
using System.Text;
using linguista_api.Globals;
using linguista_api.Models.Completions;
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

		public async Task<CompletionsResponse> SendCompletionRequest(CompletionsRequest request)
		{
            var url = "https://api.openai.com/v1/chat/completions";
            
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
    }
}