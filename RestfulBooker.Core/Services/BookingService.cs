using RestfulBooker.Core;
using RestSharp;

public class BookingService : RestClientBase
{
    public BookingService(string baseUrl) : base(baseUrl)
    {
    }

    /// <summary>
    /// Retrieves a list of all existing booking summaries. Returns an empty list if no data is found.
    /// </summary>
    /// <returns></returns>
    public async Task<List<BookingSummary>> GetAllBookingIdsAsync()
    {
        var request = new RestRequest("/booking", Method.Get);
        var data = await SendRequestAsync<List<BookingSummary>>(request);
        return data.Data ?? new List<BookingSummary>();
    }

    /// <summary>
    /// Fetches detailed information for a specific booking by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ApiResponse<BookingGetResponse>> GetBookingAsync(int id)
    {
        var request = new RestRequest($"/booking/{id}", Method.Get);
        return await SendRequestAsync<BookingGetResponse>(request);
    }

    /// <summary>
    /// Creates a new booking. 
    /// Crucially, it automatically registers the new bookingid in ResourceRegistry.
    /// IdsToDelete to ensure lifecycle management.
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    public async Task<ApiResponse<BookingCreateResponse>> CreateBookingAsync(BookingRequests payload)
    {
        var request = new RestRequest("/booking", Method.Post).AddJsonBody(payload);
        var response = await SendRequestAsync<BookingCreateResponse>(request);
        

        ResourceRegistry.IdsToDelete.Add(response.Data.bookingid);
        return response;
    }

    /// <summary>
    /// Removes a specific booking. Requires an authentication token passed via Cookie header.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task DeleteBookingAsync(int id, string token)
    {
        var request = new RestRequest($"/booking/{id}", Method.Delete)
            .AddHeader("Cookie", $"token={token}");
        await _client.ExecuteAsync(request);
    }

    /// <summary>
    /// A generic method for updating a booking. 
    /// It accepts any payload type (TRequest) 
    /// and returns a specified response type (TResponse), using a token for authorization.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="id"></param>
    /// <param name="payload"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<ApiResponse<TResponse>> UpdateBookingAsync<TRequest, TResponse>(int id, TRequest payload, string token)
        where TRequest : class
    {
        var request = new RestRequest($"/booking/{id}", Method.Put);
        request.AddHeader("Cookie", $"token={token}");
        request.AddJsonBody(payload);
        return await SendRequestAsync<TResponse>(request);
    }
}