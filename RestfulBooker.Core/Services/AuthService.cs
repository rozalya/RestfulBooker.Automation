using RestSharp;

namespace RestfulBooker.Core
{
    public class AuthService
    {
        private readonly RestClient _client;

        public AuthService(RestClient client)
        {
            _client = client;
        }

        public string GetAuthToken(string username, string password)
        {
            var request = new RestRequest("/auth", Method.Post);
            request.AddJsonBody(new { username, password });

            var response = _client.Execute<AuthResponse>(request);

            if (response.Data == null || string.IsNullOrEmpty(response.Data.Token))
                throw new Exception("Authentication failed. Check your credentials.");

            return response.Data.Token;
        }
    }
}