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
        return data.Data ?? new List<BookingSummary>();
    }
    public async Task<ApiResponse<BookingGetResponse>> GetBookingAsync(int id)
    {
        var request = new RestRequest($"/booking/{id}", Method.Get);
        return await SendRequestAsync<BookingGetResponse>(request);
    }

    public async Task<ApiResponse<BookingCreateResponse>> CreateBookingAsync(BookingRequest payload)
    {
        var request = new RestRequest("/booking", Method.Post).AddJsonBody(payload);
        var response = await SendRequestAsync<BookingCreateResponse>(request);
        
        // Successfully registers in the Core registry
        ResourceRegistry.IdsToDelete.Add(response.Data.bookingid);
        return response;
    }

    // Example of adding authentication to a specific call
    public async Task DeleteBookingAsync(int id, string token)
    {
        var request = new RestRequest($"/booking/{id}", Method.Delete)
            .AddHeader("Cookie", $"token={token}");
        await _client.ExecuteAsync(request);
    }

    public async Task<ApiResponse<BookingGetResponse>> UpdateBookingAsync(int id, BookingRequest payload, string token)
    {
        var request = new RestRequest($"/booking/{id}", Method.Put);
        request.AddHeader("Cookie", $"token={token}");
        request.AddJsonBody(payload);
        return await SendRequestAsync<BookingGetResponse>(request);      
    }
}