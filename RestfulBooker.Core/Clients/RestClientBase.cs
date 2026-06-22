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

            if (!response.IsSuccessful)
            {
                string errorDetails = response.Content ?? "No error content provided.";

                Log.Error($"API call failed: {request.Method} {request.Resource}. Status: {response.StatusCode}. Details: {errorDetails}");

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new ResourceNotFoundException($"Resource not found: {errorDetails}");

                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        throw new ApiException(response.StatusCode, $"Authorization denied: {errorDetails}");

                    case HttpStatusCode.BadRequest:
                        throw new ApiException(HttpStatusCode.BadRequest, $"Bad Request: {errorDetails}");
                   
                    case HttpStatusCode.InternalServerError:
                        throw new ApiException(HttpStatusCode.InternalServerError, $"InternalServerError: {errorDetails}");
                    default:
                        throw new ApiException(response.StatusCode, $"Unexpected error: {errorDetails}");
                }
            }
            return new ApiResponse<T>
            {
                Data = response.Data,
                StatusCode = response.StatusCode
            };
        }
    }
}