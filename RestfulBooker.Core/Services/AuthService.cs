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

        /// <summary>
        /// It sends a POST request to the /auth endpoint 
        /// containing the user's credentials (username and password) in the JSON body.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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