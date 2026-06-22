using Microsoft.Extensions.Configuration;
using RestfulBooker.Core;
using Serilog;

namespace RestfulBooker.Tests
{
    public class TestBase
    {
        protected RestfulBookerApp App;
        protected string AuthToken;
        protected IConfiguration Configuration;
        public TestBase()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

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
            Log.CloseAndFlush();
        }

        [TearDown]
        public async Task GlobalTearDown()
        {
            // 1. Perform cleanup first
            await BaseCleanup();

            // 2. Attach logs after cleanup is done
            // Note: Ensure the path is correct or dynamic based on the test
            TestContext.AddTestAttachment("logs/test-run-.log", "Execution Logs");
        }

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
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex is FluentAssertions.Execution.AssertionFailedException)
                {
                    Log.Error($">>> ASSERTION FAILED: {description}. Details: {ex.Message}");
                }
                else
                {
                    Log.Error($">>> STEP ERROR: {description}. Exception: {ex.GetType().Name} - {ex.Message}");
                }
                throw; 
            }
        }

        protected async Task StepAsync(string description, Func<Task> action)
        {
            Log.Information($">>> STEP: {description}");
             try
            {
                await action();
            }
            catch (Exception ex)
            {
                // Custom logging based on the type of error
                if (ex is FluentAssertions.Execution.AssertionFailedException)
                {
                    Log.Error($">>> ASSERTION FAILED: {description}. Details: {ex.Message}");
                }
                else
                {
                    Log.Error($">>> STEP ERROR: {description}. Exception: {ex.GetType().Name} - {ex.Message}");
                }
                throw; 
            };
        }
    }
}