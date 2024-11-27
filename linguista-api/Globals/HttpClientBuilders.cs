using Polly;
using Polly.Extensions.Http;

namespace linguista_api.Globals
{
	public static class HttpClientBuilders
	{
		public static void CreateHttpClient(this IServiceCollection services, string clientName, int retries = 2, int singleAttemptTimeout = 60,
			int clientTimeout = 120, string baseAddress = "")
		{
			List<TimeSpan> retryTimespans = new List<TimeSpan>();

			for (int i = 0; i < retries; i++)
			{
				retryTimespans.Add(TimeSpan.FromMilliseconds(500 * Math.Pow(2, 1)));
			}

			var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(singleAttemptTimeout);
			var retryPolicy = HttpPolicyExtensions
				.HandleTransientHttpError()
				.Or<TimeoutException>()
				.WaitAndRetryAsync(retryTimespans.ToArray());

			services.AddHttpClient(clientName, client =>
			{
				client.Timeout = TimeSpan.FromSeconds(clientTimeout);
				client.BaseAddress = new Uri(baseAddress);
			});
		}
	}
}