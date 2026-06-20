using RestfulBooker.Core;
using RestSharp;
using Serilog;
using static RestfulBooker.Core.BookingModel;

public class BookingService
{
    protected readonly RestClient _client;

    // The service doesn't create the client; it receives it.
    public BookingService(RestClient client)
    {
        _client = client;
    }

    public async Task<List<BookingSummary>> GetAllBookingIdsAsync()
    {
        var request = new RestRequest("/booking", Method.Get);
        var response = await _client.ExecuteAsync<List<BookingSummary>>(request);
        if (!response.IsSuccessful)
            throw new Exception($"Failed to retrieve booking list: {response.StatusCode}");

        return response.Data ?? new List<BookingSummary>();
    }
    public async Task<BookingRequest> GetBookingAsync(int id, string token)
    {
        Log.Information($"Sending GET request to /booking/{id}");
        // The path is defined as /booking/{id}
        var request = new RestRequest($"/booking/{id}", Method.Get);

        // Add Accept header to ensure the API returns JSON
        //request.AddHeader("Accept", "application/json");
        request.AddHeader("Cookie", $"token={token}");

        var response = await _client.ExecuteAsync<BookingRequest>(request);

        if (!response.IsSuccessful)
        {
            Log.Error("Request failed with status {StatusCode}. Content: {Content}",
            response.StatusCode, response.Content);
            throw new Exception($"GetBooking failed: {response.StatusCode}. Content: {response.Content}");
        }

        //Log.Information("Booking recived with ID: {BookingId}", response.Data?.bookingid);
        return response.Data;
    }

    public async Task<BookingResponse> CreateBookingAsync(BookingRequest payload, string token)
    {
        Log.Information("Sending POST request to /booking");
        var request = new RestRequest("/booking", Method.Post).AddJsonBody(payload);
        request.AddHeader("Cookie", $"token={token}");
        var response = await _client.ExecuteAsync<BookingResponse>(request);

        if (!response.IsSuccessful)
        {
            Log.Error("Request failed with status {StatusCode}. Content: {Content}",
            response.StatusCode, response.Content);
            // Throw a helpful error with the status code and raw response
            throw new Exception($"API Request failed: {response.StatusCode}. Content: {response.Content}");
        }

        // 2. Check if the deserialized data is null
        if (response.Data == null)
        {
            throw new Exception("API returned a successful status, but the response body was empty or could not be parsed.");
        }
        if (response.IsSuccessful && response.Data != null)
        {
            // Automatically register the ID for cleanup
            ResourceRegistry.IdsToDelete.Add(response.Data.bookingid);
        }
        //Log.Information("Booking created successfully with ID: {BookingId}", response.Data?.bookingid);
        return response.Data;
    }

    // Example of adding authentication to a specific call
    public async Task DeleteBookingAsync(int id, string token)
    {
        var request = new RestRequest($"/booking/{id}", Method.Delete)
            .AddHeader("Cookie", $"token={token}");


        await _client.ExecuteAsync(request);
    }

    public async Task<BookingResponse> UpdateBookingAsync(int id, BookingRequest payload, string token)
    {
        var request = new RestRequest($"/booking/{id}", Method.Put);
        request.AddHeader("Cookie", $"token={token}");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");
        request.AddJsonBody(payload);

        var response = await _client.ExecuteAsync<BookingResponse>(request);

        if (!response.IsSuccessful)
            throw new Exception($"Update failed: {response.StatusCode}. Content: {response.Content}");

        Log.Information("Successfully updated booking {Id}", id);
        return response.Data;
    }
}