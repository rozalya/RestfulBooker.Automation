namespace RestfulBooker.Core
{
    public class RestfulBookerApp : RestClientBase
    {
        public AuthService Auth { get; }
        public BookingService Booking { get; }

        public RestfulBookerApp(string baseUrl) : base(baseUrl)
        {
            Auth = new AuthService(_client);
            Booking = new BookingService(_client.Options.BaseUrl.ToString());
        }
    }
}
