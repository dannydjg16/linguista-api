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
            var apiKey = "sk-proj-seFyVlFL0J2tF3nRNyCUT3BlbkFJVulTxl84lo6NoLferWSx";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            //client.DefaultRequestHeaders.Add("Cookie", "__cf_bm=oN33XBCKzR6XoG5CKcI5CAL8HyMaUcHJA217_ZXVjy4-1717798487-1.0.1.1-8jW58KJeNDoe9a7Vm_VmlUZ_7MTQfN.tTzJyjJAkwt9DZ6_xCB3sgAYk5jQ8w0TZKRE.SS3rZLp5xJNQp4NTGQ; _cfuvid=7q.rZMXxY9iaj4PklJy4_WRbOMAjCpUD.VrFt1SN8rk-1717798487057-0.0.1.1-604800000");

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