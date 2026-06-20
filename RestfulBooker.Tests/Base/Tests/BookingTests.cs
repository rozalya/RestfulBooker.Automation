using FluentAssertions;

namespace RestfulBooker.Tests
{
    [TestFixture]
    public class BookingTests : TestBase
    {
        [Test]
        public async Task GetBooking_ValidId_ShouldReturnBookingDetails()
        {
            // Act
            var booking = await App.Booking.GetBookingAsync(1);

            // Assert
            booking.Should().NotBeNull();
            //booking.Firstname.Should().NotBeNullOrEmpty();
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
            response.bookingid.Should().NotBe(0);
        }

        [Test]
        public void UpdateBooking_WithValidToken_Returns200Ok()
        {
            /*// Arrange: Create a booking first to get a fresh ID, ensuring independence
            var createdBooking = BookingService.CreateBooking(new BookingRequest {  });
            int bookingId = createdBooking.Bookingid; //.Data.Bookingid;

            var updateData = new BookingRequest { Firstname = "UpdatedName" };

            // Act
            var response = BookingService.UpdateBooking(bookingId, updateData, AuthToken);*/

            // Assert
          /*  response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Data.Firstname.Should().Be("UpdatedName"); //*/
        }
    }
}
