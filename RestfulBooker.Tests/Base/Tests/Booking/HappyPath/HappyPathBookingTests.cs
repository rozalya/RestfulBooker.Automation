using FluentAssertions;
using RestfulBooker.Core;
using RestfulBooker.Tests;
using System.Net;

namespace RestfulBooker.Tests
{
    [TestFixture]
    public class HappyPathBookingTests : TestBase
    {
        [Test]
        public async Task GetBooking_ValidId_ShouldReturnBookingDetails()
        {
            int firstId = 0;
            ApiResponse<BookingGetResponse> booking = null;

            await StepAsync("Arrange: Get booking ID", async () =>
            {
                var bookingList = await App.Booking.GetAllBookingIdsAsync();
                firstId = bookingList.First().bookingId;
            });

            await StepAsync("Act: Use the ID just captured", async () =>
            {
                booking = await App.Booking.GetBookingAsync(firstId);
            });

            Step("Assert: assert the response received is expected", () =>
            {
                booking.StatusCode.Should().Be(HttpStatusCode.OK);
                booking.Data.Should().BeValid("because we expect a valid response from the API");
            });
        }

        [Test]
        public async Task CreateBooking_WithValidData_Returns200OkAsync()
        {
            BookingRequests bookingData = null;
            ApiResponse<BookingCreateResponse> response = null;

            Step("Arrange: generate booking data", () =>
            {
                bookingData = BookingDataGenerator.GetBookingFaker().Generate();
            });

            await StepAsync("Act: create booking with the data generated data", async () =>
            {
                response = await App.Booking.CreateBookingAsync(bookingData);
            });

            Step("Assert: assert the response received is expected", () =>
            {
                response.Data.Should().NotBeNull();
                response.Data.Should().BeValid("because we expect a valid response from the API");
            });
        }

        [Test]
        public async Task UpdateBooking_ValidData_ShouldReturnUpdatedDetails()
        {
            BookingRequests updatedData = null;
            ApiResponse<BookingCreateResponse> createdBooking = null;

            await StepAsync("Arrange: generate booking data and create booking", async () =>
            {
                var initialData = BookingDataGenerator.GetBookingFaker().Generate();
                createdBooking = await App.Booking.CreateBookingAsync(initialData);
            });

            await StepAsync("Act: Prepare updated data and perform the PUT request", async () =>
            {
                updatedData = BookingDataGenerator.GetBookingFaker().Generate();
                await App.Booking.UpdateBookingAsync<BookingRequests, BookingGetResponse>(createdBooking.Data.bookingid, updatedData, AuthToken);
            });

            await StepAsync("Assert: assert the response received is expected", async () =>
            {
                var retrievedBooking = await App.Booking.GetBookingAsync(createdBooking.Data.bookingid);
                retrievedBooking.Data.Should().Match(updatedData);
            });
        }
    }
}
