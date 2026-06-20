using Microsoft.Extensions.Configuration;
using RestfulBooker.Core;
using Serilog;

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

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/test-run-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
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

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            // Ensure logs are flushed to disk before the process exits
            Log.CloseAndFlush();
        }

        [TearDown]
       /* public void LogAttachment()
        {
            // Attaches the log file to the current test result for easier viewing in reporting tools
            //TestContext.AddTestAttachment("logs/test-run-.log", "Execution Logs");
        }*/
        public async Task BaseCleanup()
        {
            foreach (var id in ResourceRegistry.IdsToDelete)
            {
                try
                {
                    await App.Booking.DeleteBookingAsync(id, AuthToken);
                    Log.Information("Successfully cleaned up ID: {Id}", id);
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to clean up ID {Id}: {Message}", id, ex.Message);
                }
            }
            ResourceRegistry.IdsToDelete.Clear();
        }

        // 2. UTILITY METHODS (Called manually by your tests)
        protected void Step(string description, Action action)
        {
            Log.Information($">>> STEP: {description}");
            action();
        }
        // Overload for Async steps
        protected async Task StepAsync(string description, Func<Task> action)
        {
            Log.Information($">>> STEP: {description}");
            await action();
        }
    }
}