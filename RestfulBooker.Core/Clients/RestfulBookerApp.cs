

namespace RestfulBooker.Core
{
    public class RestfulBookerApp : RestClientBase
    {
        public AuthService Auth { get; }
        public BookingService Booking { get; }

        public RestfulBookerApp(string baseUrl) : base(baseUrl)
        {
            // Now you can pass the 'Client' inherited from RestClientBase
            // to your services so they all share the same configuration
            Auth = new AuthService(Client);
            Booking = new BookingService(Client);
        }
    }
}
