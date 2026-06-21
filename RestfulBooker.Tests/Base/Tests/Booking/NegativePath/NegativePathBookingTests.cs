using FluentAssertions;
using RestfulBooker.Core;
using System.Net;
using static RestfulBooker.Core.BookingModel;

namespace RestfulBooker.Tests
{
    [TestFixture]
    public class NegativePathBookingTests : TestBase
    {
        [Test]
        public async Task GetBooking_WhenIdDoesNotExist_Returns404()
        {
            int firstId = 0;
            Func<Task> booking = null;

            await StepAsync("Arrange: Get booking ID", async () =>
            {
                var bookingList = await App.Booking.GetAllBookingIdsAsync();
                firstId = bookingList.First().bookingId;
            });
            await StepAsync("Act: Use the ID we just captured", async () =>
            {
                booking = async () => await App.Booking.GetBookingAsync(firstId + 9999999);
            });

            Step("Assert", () =>
            {
                booking.Should().ThrowAsync<ApiException>()
                   .WithMessage("*Bad Request*") 
                   .Where(e => e.StatusCode == HttpStatusCode.BadRequest);

            });          
        }
        [Test]
        public async Task CreateBooking_MissingRequiredFields_Returns500()
        {

            BookingRequest bookingData = null;
            Func<Task> response = null;
            Step("Arrange", () =>
            {
                bookingData = BookingDataGenerator.GetBookingFaker().Generate();
                bookingData.lastname = null;
                bookingData.firstname = null;
                bookingData.additionalneeds = null;
            });

            await StepAsync("Act:", async () =>
            {
                response = async () => await App.Booking.CreateBookingAsync(bookingData);
            });

            Step("Assert", () =>
            {
                response.Should().ThrowAsync<ApiException>()
                   .WithMessage("*Internal Server Error*")
                   .Where(e => e.StatusCode == HttpStatusCode.InternalServerError);
            });


            // Assert
            //response.StatusCode.Should().Be(HttpStatusCode.InternalServerError, "защото сървърът не може да обработи непълна заявка");
        }

        [Test]
        public async Task CreateBooking_InvalidDateFormat_Returns500()
        {
            BookingRequest bookingData = null;
            Func<Task> response = null;
            Step("Arrange", () =>
            {
                bookingData = BookingDataGenerator.GetBookingFaker().Generate();
                bookingData.bookingdates.checkin = "not-a-date";

            });

            await StepAsync("Act:", async () =>
            {
                response = async () => await App.Booking.CreateBookingAsync(bookingData);
            });

            Step("Assert", () =>
            {
                response.Should().ThrowAsync<ApiException>()
                   .WithMessage("*Internal Server Error*")
                   .Where(e => e.StatusCode == HttpStatusCode.InternalServerError);

            });

        }

        [Test]
        public async Task CreateBooking_EmptyBody_Returns500Or400()
        {
            BookingRequest emptyPayload = null;
            Func<Task> response = null;

            await StepAsync("Act:", async () =>
            {
                response = async () => await App.Booking.CreateBookingAsync(emptyPayload);
            });

            Step("Assert", () =>
            {
                response.Should().ThrowAsync<ApiException>()
                  .Where(e => e.StatusCode == HttpStatusCode.BadRequest ||
                         e.StatusCode == HttpStatusCode.InternalServerError);
            }); 
        }
    }
}
