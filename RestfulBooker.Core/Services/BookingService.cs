using RestfulBooker.Core;
using RestSharp;
using Serilog;
using static RestfulBooker.Core.BookingModel;

public class BookingService : RestClientBase
{
    public BookingService(string baseUrl) : base(baseUrl)
    {
    }


    public async Task<List<BookingSummary>> GetAllBookingIdsAsync()
    {
        var request = new RestRequest("/booking", Method.Get);
        var data = await SendRequestAsync<List<BookingSummary>>(request);
        return data ?? new List<BookingSummary>();
    }
    public async Task<BookingRequest> GetBookingAsync(int id)
    {
        var request = new RestRequest($"/booking/{id}", Method.Get);
        return await SendRequestAsync<BookingRequest>(request);
    }

    public async Task<BookingResponse> CreateBookingAsync(BookingRequest payload)
    {
        var request = new RestRequest("/booking", Method.Post).AddJsonBody(payload);
        return await SendRequestAsync<BookingResponse>(request);      
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
        request.AddJsonBody(payload);
        return await SendRequestAsync<BookingResponse>(request);      
    }
}