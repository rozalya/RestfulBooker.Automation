using FluentAssertions;
using RestfulBooker.Core;
using RestfulBooker.Tests;
using System.Net;
using static RestfulBooker.Core.BookingModel;

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

            await StepAsync("Act: Use the ID we just captured", async () =>
            {
                booking = await App.Booking.GetBookingAsync(firstId);
            });

            Step("Assert", () =>
            {
                booking.StatusCode.Should().Be(HttpStatusCode.OK);
                booking.Data.Should().BeValid("because we expect a valid response from the API");
            });
        }

        [Test]
        public async Task CreateBooking_WithValidData_Returns200OkAsync()
        {
            BookingRequest bookingData = null;
            ApiResponse<BookingCreateResponse> response = null;

            Step("Arrange", () =>
            {
                bookingData = BookingDataGenerator.GetBookingFaker().Generate();
            });

            await StepAsync("Act:", async () =>
            {
                response = await App.Booking.CreateBookingAsync(bookingData);
            });

            Step("Assert", () =>
            {
                response.Data.Should().NotBeNull();
                response.Data.Should().BeValid("because we expect a valid response from the API");
            });
        }

        [Test]
        public async Task UpdateBooking_ValidData_ShouldReturnUpdatedDetails()
        {
            BookingRequest updatedData = null;
            ApiResponse<BookingCreateResponse> createdBooking = null;

            await StepAsync("Arrange: Create a booking so we have something to update", async () =>
            {
                var initialData = BookingDataGenerator.GetBookingFaker().Generate();
                createdBooking = await App.Booking.CreateBookingAsync(initialData);
            });

            await StepAsync("Act: Prepare updated data and perform the PUT request", async () =>
            {
                updatedData = BookingDataGenerator.GetBookingFaker().Generate();
                await App.Booking.UpdateBookingAsync(createdBooking.Data.bookingid, updatedData, AuthToken);
            });

            await StepAsync("Assert: Get the booking again to verify the update", async () =>
            {
                var retrievedBooking = await App.Booking.GetBookingAsync(createdBooking.Data.bookingid);

                retrievedBooking.Data.Should().Match(updatedData);
                /* retrievedBooking.firstname.Should().Be(updatedData.firstname);
                 retrievedBooking.lastname.Should().Be(updatedData.lastname);
                 retrievedBooking.totalprice.Should().Be(updatedData.totalprice);*/
            });
        }
    }
}
