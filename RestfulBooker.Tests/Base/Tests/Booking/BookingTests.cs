using FluentAssertions;
using static RestfulBooker.Core.BookingModel;

namespace RestfulBooker.Tests.Base.Tests.Booking
{
    [TestFixture]
    public class BookingTests : TestBase
    {
        [Test]
        public async Task GetBooking_ValidId_ShouldReturnBookingDetails()
        {
            int firstId = 0;
            BookingRequest booking = null;

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
                booking.Should().NotBeNull();
                booking.firstname.Should().NotBeNull();
            });
        }

        [Test]
        public async Task CreateBooking_WithValidData_Returns200OkAsync()
        {
            BookingRequest bookingData = null;
            BookingResponse response = null;

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
                response.Should().NotBeNull();
                response.booking.firstname.Should().Be(bookingData.firstname);
                response.booking.lastname.Should().Be(bookingData.lastname);
                response.booking.bookingdates.checkin.Should().Be(bookingData.bookingdates.checkin);
                response.booking.bookingdates.checkout.Should().Be(bookingData.bookingdates.checkout);
            });
        }

        [Test]
        public async Task UpdateBooking_ValidData_ShouldReturnUpdatedDetails()
        {
            BookingRequest initialData = null;
            BookingRequest updatedData = null;
            BookingResponse createdBooking = null;

            await StepAsync("Arrange: Create a booking so we have something to update", async () =>
            {
                var initialData = BookingDataGenerator.GetBookingFaker().Generate();
                createdBooking = await App.Booking.CreateBookingAsync(initialData);
            });

            await StepAsync("Act: Prepare updated data and perform the PUT request", async () =>
            {
                updatedData = BookingDataGenerator.GetBookingFaker().Generate();
                await App.Booking.UpdateBookingAsync(createdBooking.bookingid, updatedData, AuthToken);
            });

            await StepAsync("Assert: Get the booking again to verify the update", async () =>
            {
                var retrievedBooking = await App.Booking.GetBookingAsync(createdBooking.bookingid);

                retrievedBooking.firstname.Should().Be(updatedData.firstname);
                retrievedBooking.lastname.Should().Be(updatedData.lastname);
                retrievedBooking.totalprice.Should().Be(updatedData.totalprice);
            });
        }
    }
}
