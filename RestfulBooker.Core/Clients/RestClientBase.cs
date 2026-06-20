using RestSharp;

namespace RestfulBooker.Core
{
    public class RestClientBase
    {
        protected readonly RestClient Client;

        public RestClientBase(string baseUrl)
        {
            var options = new RestClientOptions(baseUrl)
            {
                // You can add global configurations here
                Timeout = TimeSpan.FromSeconds(10), // 10 seconds timeout
            };

            Client = new RestClient(options);
            Client.AddDefaultHeader("Content-Type", "application/json");
            Client.AddDefaultHeader("Accept", "application/json");
        }
    }
}