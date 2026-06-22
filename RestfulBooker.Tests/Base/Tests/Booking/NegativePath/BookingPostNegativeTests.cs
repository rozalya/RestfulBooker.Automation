using FluentAssertions;
using RestfulBooker.Core;
using System.Net;

namespace RestfulBooker.Tests
{
    [TestFixture]
    public class BookingPostNegativeTests : TestBase
    {
        [Test]
        public async Task GetBooking_WhenIdDoesNotExist_Returns404()
        {
            int firstId = 0;
            ApiResponse<BookingGetResponse> booking = null;

            await StepAsync("Arrange: Get booking ID", async () =>
            {
                var bookingList = await App.Booking.GetAllBookingIdsAsync();
                firstId = bookingList.First().bookingId;
            });
            await StepAsync("Act: Use the ID to modify it and get booking", async () =>
            {
                booking = await App.Booking.GetBookingAsync(firstId + 9999999);
            });

            Step("Assert", () =>
            {
                booking.StatusCode.Should().Be(
                    HttpStatusCode.NotFound,
                    $"the server should return a 404 error for bad request. Received:" +
                    $" {booking.StatusCode} {(int)booking.StatusCode}");
            });          
        }
        [Test]
        public async Task CreateBooking_MissingRequiredFields_Returns500()
        {

            BookingRequests bookingData = null;
            ApiResponse<BookingCreateResponse> response = null;
            Step("Arrange: generate booking data", () =>
            {
                bookingData = BookingDataGenerator.GetBookingFaker().Generate();
                bookingData.lastname = "";
                bookingData.firstname = "";
                bookingData.additionalneeds = "";
            });

            await StepAsync("Act: create booking with the data generated data", async () =>
            {
                response = await App.Booking.CreateBookingAsync(bookingData);
            });

           await StepAsync("Assert: assert the response received is expected", async () =>
            {              
                response.StatusCode.Should().Be(
                    HttpStatusCode.InternalServerError,
                    $"the server should return a 500 error for empty data. Received:" +
                    $" {response.StatusCode} {(int)response.StatusCode}");
            });           
        }

        [Test]
        public async Task CreateBooking_InvalidDateFormat_Returns500()
        {
            BookingRequests bookingData = null;
            ApiResponse<BookingCreateResponse> response = null;
            Step("Arrange: generate booking data", () =>
            {
                bookingData = BookingDataGenerator.GetBookingFaker().Generate();
                bookingData.bookingdates.checkin = "not-a-date";
                bookingData.bookingdates.checkout = "not-a-date";
            });

            await StepAsync("Act: create booking with the data generated data", async () =>
            {
                response =  await App.Booking.CreateBookingAsync(bookingData);
            });

            Step("Assert: assert the response received is expected", () =>
            {
                response.StatusCode.Should().Be(
                   HttpStatusCode.InternalServerError,
                   $"the server should return a 500 error for empty data. Received:" +
                   $" {response.StatusCode} {(int)response.StatusCode}");
            });

        }

        [Test]
        public async Task CreateBooking_InvalidTotalPrise()
        {
            BookingRequests bookingData = null;
            ApiResponse<BookingCreateResponse> response = null;
            Step("Arrange: generate booking data", () =>
            {
                bookingData = BookingDataGenerator.GetBookingFaker().Generate();
                bookingData.totalprice = -234;
                bookingData.depositpaid = true;
            });

            await StepAsync("Act: create booking with the data generated data", async () =>
            {
                response = await App.Booking.CreateBookingAsync(bookingData);
            });

            Step("Assert: assert the response received is expected", () =>
            {
                response.StatusCode.Should().Be(
                   HttpStatusCode.InternalServerError,
                   $"the server should return a 500 error for empty data. Received:" +
                   $" {response.StatusCode} {(int)response.StatusCode}");
            });
        }

        [Test]
        public async Task CreateBooking_EmptyStringName()
        {
            BookingRequests bookingData = null;
            ApiResponse<BookingCreateResponse> response = null;
            Step("Arrange: generate booking data ", () =>
            {
                bookingData = BookingDataGenerator.GetBookingFaker().Generate();
                bookingData.lastname = "";
                bookingData.firstname = "";
                bookingData.additionalneeds = "";
            });

            await StepAsync("Act: create booking with the data generated data", async () =>
            {
                response = await App.Booking.CreateBookingAsync(bookingData);
            });

            Step("Assert: assert the response received is expected", () =>
            {
                response.StatusCode.Should().Be(
                   HttpStatusCode.InternalServerError,
                   $"the server should return a 500 error for empty data. Received:" +
                   $" {response.StatusCode} {(int)response.StatusCode}");
            });         
        }

        [Test]
        public async Task CreateBooking_EmptyBody_Returns500Or400()
        {
            BookingRequests emptyPayload = null;
            ApiResponse<BookingCreateResponse> response = null;

            Step("Arrange: generate booking data", () =>
            {
                emptyPayload = new BookingRequests() { };
            });
            await StepAsync("Act: create booking with the data generated data", async () =>
            {                              
                response =  await App.Booking.CreateBookingAsync(emptyPayload);
            });
            await StepAsync("Assert: assert the response received is expected", async () =>
            {
                response.StatusCode.Should().BeOneOf(
                 new[] { HttpStatusCode.InternalServerError, HttpStatusCode.BadRequest },
                      "the server should return a 500 or 400 error for empty data. Received:" +
                      $" {response.StatusCode} {(int)response.StatusCode}");
            });
        }

        [Test]
        public async Task CreateBooking_WithoutAuth_ShouldThrowUnauthorized()
        {
            BookingRequests updatedData = null;
            ApiResponse<BookingCreateResponse> createdBooking = null;
            ApiResponse<BookingGetResponse> act = null;

            await StepAsync("Arrange: generate booking data and create booking", async () =>
            {
                var initialData = BookingDataGenerator.GetBookingFaker().Generate();
                createdBooking = await App.Booking.CreateBookingAsync(initialData);
            });

            await StepAsync("Act: Prepare updated data and perform the PUT request", async () =>
            {
                updatedData = BookingDataGenerator.GetBookingFaker().Generate();
                act =  await App.Booking.UpdateBookingAsync<BookingRequests, BookingGetResponse>(createdBooking.Data.bookingid, updatedData, token: "testToken");
            });

            await StepAsync("Asser: assert the response received is expected", async () =>
            {
                act.StatusCode.Should().BeOneOf(
                 new[] { HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized },
                      "the server should return a 401 or 403 error for empty data. Received:" +
                      $" {act.StatusCode} {(int)act.StatusCode}");
            });
        }

    }
}
