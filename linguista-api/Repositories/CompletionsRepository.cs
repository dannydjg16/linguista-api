using System;
using System.Net.Http.Headers;
using System.Text;
using linguista_api.Models.Completions;
using linguista_api.Repositories.Interfaces;
using Newtonsoft.Json;

namespace linguista_api.Repositories
{
	public class CompletionsRepository: ICompletionsRepository
	{
        private static readonly HttpClient client = new HttpClient();
		private readonly IHttpClientFactory _factory;

        public CompletionsRepository(IHttpClientFactory factory)
		{
			_factory = factory;
		}

		public async Task<CompletionsResponse> SendCompletionRequest()
		{
            var url = "https://api.openai.com/v1/chat/completions";
            

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", a);

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You will be provided with statements, and your task is to convert them to standard English." },
                    new { role = "user", content = "She no went to the market." }
                },
                temperature = 0.2,
                max_tokens = 40,
                top_p = 1
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            var responseObject = JsonConvert.DeserializeObject<CompletionsResponse>(responseString);

            return responseObject;
        }
    }
}