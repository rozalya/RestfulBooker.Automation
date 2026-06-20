using FluentAssertions;

namespace RestfulBooker.Tests
{
    [TestFixture]
    public class BookingTests : TestBase
    {
        [Test]
        public async Task GetBooking_ValidId_ShouldReturnBookingDetails()
        {
            // 1. Arrange: Get booking ID
            var bookingList = await App.Booking.GetAllBookingIdsAsync();
            var firstId = bookingList.First().bookingId;

            // 2. Act: Use the ID we just captured
            var booking = await App.Booking.GetBookingAsync(firstId, AuthToken);

            // 3. Assert
            booking.Should().NotBeNull();
            booking.firstname.Should().NotBeNull();
        }


        [Test]
        public async Task CreateBooking_WithValidData_Returns200OkAsync()
        {
            // Arrange
            var bookingData = BookingDataGenerator.GetBookingFaker().Generate();

            // Act
            var response = await App.Booking.CreateBookingAsync(bookingData, AuthToken);

            // Assert
            response.Should().NotBeNull();
            response.booking.firstname.Should().Be(bookingData.firstname);
            response.booking.lastname.Should().Be(bookingData.lastname);
            response.booking.bookingdates.checkin.Should().Be(bookingData.bookingdates.checkin);
            response.booking.bookingdates.checkout.Should().Be(bookingData.bookingdates.checkout);
        }

        [Test]
        public async Task UpdateBooking_ValidData_ShouldReturnUpdatedDetails()
        {
            // 1. Arrange: Create a booking so we have something to update
            var initialData = BookingDataGenerator.GetBookingFaker().Generate();
            var createdBooking = await App.Booking.CreateBookingAsync(initialData, AuthToken);

            // 2. Act: Prepare updated data and perform the PUT request
            var updatedData = BookingDataGenerator.GetBookingFaker().Generate();
            await App.Booking.UpdateBookingAsync(createdBooking.bookingid, updatedData, AuthToken);

            // 3. Assert: Get the booking again to verify the update
            var retrievedBooking = await App.Booking.GetBookingAsync(createdBooking.bookingid, AuthToken);

            // Verify the fields were actually updated
            retrievedBooking.firstname.Should().Be(updatedData.firstname);
            retrievedBooking.lastname.Should().Be(updatedData.lastname);
            retrievedBooking.totalprice.Should().Be(updatedData.totalprice);
        }
    }
}
