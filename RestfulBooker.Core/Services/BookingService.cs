using Newtonsoft.Json.Linq;
using RestSharp;
using static RestfulBooker.Core.BookingModel;

public class BookingService
{
    protected readonly RestClient _client;

    // The service doesn't create the client; it receives it.
    public BookingService(RestClient client)
    {
        _client = client;
    }

    public async Task<BookingResponse> GetBookingAsync(int id)
    {
        // The path is defined as /booking/{id}
        var request = new RestRequest($"/booking/{id}", Method.Get);

        // Add Accept header to ensure the API returns JSON
        request.AddHeader("Accept", "application/json");

        var response = await _client.ExecuteAsync<BookingResponse>(request);

        if (!response.IsSuccessful)
        {
            throw new Exception($"GetBooking failed: {response.StatusCode}. Content: {response.Content}");
        }

        return response.Data;
    }

    public async Task<BookingResponse> CreateBookingAsync(BookingRequest payload, string token)
    {
        var request = new RestRequest("/booking", Method.Post).AddJsonBody(payload);
        request.AddHeader("Cookie", $"token={token}");
        var response = await _client.ExecuteAsync<BookingResponse>(request);

        if (!response.IsSuccessful)
        {
            // Throw a helpful error with the status code and raw response
            throw new Exception($"API Request failed: {response.StatusCode}. Content: {response.Content}");
        }

        // 2. Check if the deserialized data is null
        if (response.Data == null)
        {
            throw new Exception("API returned a successful status, but the response body was empty or could not be parsed.");
        }
        return response.Data;
    }

    // Example of adding authentication to a specific call
    public async Task DeleteBookingAsync(int id, string token)
    {
        var request = new RestRequest($"/booking/{id}", Method.Delete)
            .AddHeader("Cookie", $"token={token}");


        await _client.ExecuteAsync(request);
    }
}