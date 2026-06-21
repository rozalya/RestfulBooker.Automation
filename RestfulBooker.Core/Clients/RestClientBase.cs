using RestSharp;
using Serilog;

namespace RestfulBooker.Core
{
    public class RestClientBase
    {
        protected readonly RestClient _client;

        public RestClientBase(string baseUrl)
        {
            var options = new RestClientOptions(baseUrl)
            {
                // You can add global configurations here
                Timeout = TimeSpan.FromSeconds(10), // 10 seconds timeout
            };

            _client = new RestClient(options);
            _client.AddDefaultHeader("Content-Type", "application/json");
            _client.AddDefaultHeader("Accept", "application/json");
        }

        protected async Task<ApiResponse<T>> SendRequestAsync<T>(RestRequest request)
        {
            var response = await _client.ExecuteAsync<T>(request);

            if (!response.IsSuccessful)
            {
                Log.Error("API call failed: {Method} {Resource}. Status: {Status}. Content: {Content}",
                    request.Method, request.Resource, response.StatusCode, response.Content);

                throw new Exception($"Request failed: {response.StatusCode}");
            }
            return new ApiResponse<T>
            {
                Data = response.Data,
                StatusCode = response.StatusCode
            };
        }
    }
}