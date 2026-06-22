using RestfulBooker.Core;
using RestSharp;

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

    public async Task<ApiResponse<BookingCreateResponse>> CreateBookingAsync(BookingRequests payload)
    {
        var request = new RestRequest("/booking", Method.Post).AddJsonBody(payload);
        var response = await SendRequestAsync<BookingCreateResponse>(request);
        

        ResourceRegistry.IdsToDelete.Add(response.Data.bookingid);
        return response;
    }

    public async Task DeleteBookingAsync(int id, string token)
    {
        var request = new RestRequest($"/booking/{id}", Method.Delete)
            .AddHeader("Cookie", $"token={token}");
        await _client.ExecuteAsync(request);
    }

    public async Task<ApiResponse<BookingCreateResponse>> CreateBookingAsync1(BookingRequests payload, string username, string password)
    {
        var request = new RestRequest("/booking", Method.Post).AddJsonBody(payload);
        request.AddJsonBody(new { username = username, password = password });
        var response = await SendRequestAsync<BookingCreateResponse>(request);

        return response;
    }
    public async Task<ApiResponse<TResponse>> UpdateBookingAsync<TRequest, TResponse>(int id, TRequest payload, string token)
        where TRequest : class
    {
        var request = new RestRequest($"/booking/{id}", Method.Put);
        request.AddHeader("Cookie", $"token={token}");
        request.AddJsonBody(payload);
        return await SendRequestAsync<TResponse>(request);
    }
}