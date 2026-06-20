namespace RestfulBooker.Core
{
    public class AuthResponse
    {
        // RestSharp will automatically map the "token" JSON field to this property
        public string Token { get; set; }
    }
}
