using FluentAssertions;
using RestfulBooker.Core;
using RestSharp;
using System.Net;
using static RestfulBooker.Core.BookingModel;

namespace RestfulBooker.Tests
{
    public class BookingPutNegativeTests : TestBase
    {
        [Test]
        public async Task UpdateBooking_WithoutAuth_ShouldThrowUnauthorized()
        {
            BookingRequest updatedData = null;
            ApiResponse<BookingCreateResponse> createdBooking = null;
            Func<Task> act = null;

            await StepAsync("Arrange: Create a booking so we have something to update", async () =>
            {
                var initialData = BookingDataGenerator.GetBookingFaker().Generate();
                createdBooking = await App.Booking.CreateBookingAsync(initialData);
            });

            await StepAsync("Act: Prepare updated data and perform the PUT request", async () =>
            {
                updatedData = BookingDataGenerator.GetBookingFaker().Generate();
                act = async () => await App.Booking.UpdateBookingAsync(createdBooking.Data.bookingid, updatedData, token: "testToken");
            });

            await StepAsync("Act:", async () =>
            {
                await act.Should().ThrowAsync<ApiException>()
                         .Where(e => e.StatusCode == HttpStatusCode.Forbidden ||
                                     e.StatusCode == HttpStatusCode.Unauthorized);
            });
        }

        [Test]
        public async Task UpdateBooking_WithInvalidDateFormat_ShouldThrowBadRequest()
        {
            BookingRequest updatedData = null;
            ApiResponse<BookingCreateResponse> createdBooking = null;
            Func<Task> act = null;

            await StepAsync("Arrange: Create a booking so we have something to update", async () =>
            {
                var initialData = BookingDataGenerator.GetBookingFaker().Generate();
                createdBooking = await App.Booking.CreateBookingAsync(initialData);
            });

            await StepAsync("Act: Prepare updated data and perform the PUT request", async () =>
            {
                updatedData = BookingDataGenerator.GetBookingFaker().Generate();
                updatedData.bookingdates.checkin = "not-a-date";            
                act = async () => await App.Booking.UpdateBookingAsync(createdBooking.Data.bookingid, updatedData, AuthToken);
            });

            await StepAsync("Act:", async () =>
            {
                await act.Should().ThrowAsync<ApiException>()
                   .Where(e => e.StatusCode == HttpStatusCode.BadRequest);
            });
        }

        [Test]
        public async Task UpdateBooking_NonExistentId_ShouldThrowNotFound()
        {
            BookingRequest updatedData = null;
            int firstId = 0;
            Func<Task> booking = null;

            await StepAsync("Arrange: Get booking ID", async () =>
            {
                var bookingList = await App.Booking.GetAllBookingIdsAsync();
                firstId = bookingList.First().bookingId;
                updatedData = BookingDataGenerator.GetBookingFaker().Generate();
            });
            await StepAsync("Act: Use the ID we just captured", async () =>
            {               
                booking = async () => await App.Booking.UpdateBookingAsync(firstId + 9999999, updatedData, AuthToken);
            });

            await StepAsync("Act: Use the ID we just captured", async () =>
            {
                await booking.Should().ThrowAsync<ApiException>()
                      .Where(e => e.StatusCode == HttpStatusCode.MethodNotAllowed);
            });  
        }
    }
}
