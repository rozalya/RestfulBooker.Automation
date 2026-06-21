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
                // Извличаме съдържанието на грешката (ако има такова)
                string errorDetails = response.Content ?? "No error content provided.";

                Log.Error("API call failed: {Method} {Resource}. Status: {Status}. Details: {Details}",
                    request.Method, request.Resource, response.StatusCode, errorDetails);

                // Тук преценяваме според кода как да реагираме
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new ResourceNotFoundException($"Ресурсът не е намерен: {errorDetails}");

                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        throw new ApiException(response.StatusCode, $"Оторизацията е отказана: {errorDetails}");

                    case HttpStatusCode.BadRequest:
                        throw new ApiException(HttpStatusCode.BadRequest, $"Лоша заявка (Bad Request): {errorDetails}");
                   
                    case HttpStatusCode.InternalServerError:
                        throw new ApiException(HttpStatusCode.InternalServerError, $"InternalServerError: {errorDetails}");
                    default:
                        throw new ApiException(response.StatusCode, $"Неочаквана грешка: {errorDetails}");
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