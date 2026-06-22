using FluentAssertions;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace RestfulBooker.Tests
{
    [TestFixture]
    public class AuthNegativePathTests 
    {
        private RestClient _client;
        private const string BaseUrl = "https://restful-booker.herokuapp.com";

        [SetUp]
        public void Setup()
        {
            _client = new RestClient(BaseUrl);
        }


        [TestCase("invalidUser", "password123")]
        [TestCase("admin", "wrongPassword")]
        [TestCase("", "")]
        [Test]
        public async Task Auth_WithInvalidCredentials_ShouldReturnBadResponse(string username, string password)
        {
            // Arrange
            var request = new RestRequest("/auth", Method.Post);
            var body = new { username, password };
            request.AddJsonBody(body);

            // Act
            var response = await _client.ExecuteAsync(request);
            // If the API returns a JSON error message, validate it
            var content = JsonConvert.DeserializeObject<dynamic>(response.Content);
            ((string)content?.reason).Should().Be("Bad credentials");
             response.StatusCode.Should().NotBe(HttpStatusCode.OK);
        }


        [Test]
        [TestCase("text/plain")]
        [TestCase("application/xml")]
        [TestCase("multipart/form-data")]
        public async Task Auth_WithUnsupportedContentType_ShouldReturn415(string contentType)
        {
            var request = new RestRequest("/auth", Method.Post);
            request.AddHeader("Content-Type", contentType); // Attempting to force a wrong header
            request.AddJsonBody(new { username = "admin", password = "password123" });

            var response = await _client.ExecuteAsync(request);

            // Expecting 415 Unsupported Media Type or 400 Bad Request
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.UnsupportedMediaType,
                HttpStatusCode.BadRequest
            );
        }


    }
}
