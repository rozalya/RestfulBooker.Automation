using RestSharp;
using Serilog;
using System.Net;

namespace RestfulBooker.Core
{
    public class RestClientBase
    {
        protected readonly RestClient _client;

        public RestClientBase(string baseUrl)
        {
            var options = new RestClientOptions(baseUrl)
            {
                Timeout = TimeSpan.FromSeconds(10), 
            };

            _client = new RestClient(options);
            _client.AddDefaultHeader("Content-Type", "application/json");
            _client.AddDefaultHeader("Accept", "application/json");
        }

        public async Task<ApiResponse<T>> SendRequestAsync<T>(RestRequest request)
        {
            var response = await _client.ExecuteAsync<T>(request);
            var apiResponse = new ApiResponse<T>
            {
                Data = response.Data,
                StatusCode = response.StatusCode
            };

            if (!response.IsSuccessful)
            {
                apiResponse.ErrorMessage = response.Content ?? "No error content provided.";
                Log.Error($"API call failed: {request.Method} {request.Resource}. Status: {response.StatusCode}. Details: {apiResponse.ErrorMessage}");
            }
            return apiResponse;
        }
    }
}