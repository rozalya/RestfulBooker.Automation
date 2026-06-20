using Microsoft.Extensions.Configuration;
using RestfulBooker.Core;

namespace RestfulBooker.Tests
{
    public class TestBase
    {
        // This 'App' gives you access to all your services (Auth, Booking, etc.)
        protected RestfulBookerApp App;
        protected string AuthToken;
        protected IConfiguration Configuration;
        public TestBase()
        {
            // Build the configuration
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            var baseUrl = Configuration["ApiSettings:BaseUrl"];
            var username = Configuration["ApiSettings:Username"];
            var password = Configuration["ApiSettings:Password"];

            App = new RestfulBookerApp(baseUrl);
            AuthToken = App.Auth.GetAuthToken(username, password);
        }
    }
}