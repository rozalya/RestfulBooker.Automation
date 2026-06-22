using FluentAssertions;
using RestfulBooker.Core;
using System.Net;

namespace RestfulBooker.Tests
{
    public class BookingPutNegativeTests : TestBase
    {
        [Test]
        public async Task UpdateBooking_WithoutAuth_ShouldThrowUnauthorized()
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

            await StepAsync("Assert: assert the response received is expected ", async () =>
            {
                act.StatusCode.Should().BeOneOf(
                 new[] { HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized },
                      "the server should return a 401 or 403 error for empty data. Received:" +
                      $" {act.StatusCode} {(int)act.StatusCode}");
            });
        }

        [Test]
        public async Task UpdateBooking_WithInvalidDateFormat_ShouldThrowBadRequest()
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
                updatedData.bookingdates.checkin = "not-a-date";            
                updatedData.bookingdates.checkout = "not-a-date";                       
                act =  await App.Booking.UpdateBookingAsync<BookingRequests, BookingGetResponse>(createdBooking.Data.bookingid, updatedData, AuthToken);
            });

            await StepAsync("Assert: assert the response received is expected ", async () =>
            {
                act.StatusCode.Should().Be(
                   HttpStatusCode.BadRequest,
                   $"the server should return a 400 error for empty data. Received:" +
                   $" {act.StatusCode} {(int)act.StatusCode}");
            });
        }

        [Test]
        public async Task UpdateBooking_WithInvalidNameFormat_ShouldThrowBadRequest()
        {
            BookingRequests updatedData = null;
            ApiResponse<BookingCreateResponse> createdBooking = null;
            ApiResponse<BookingGetResponse> booking = null;

            await StepAsync("Arrange: generate booking data and create booking", async () =>
            {
                var initialData = BookingDataGenerator.GetBookingFaker().Generate();
                createdBooking = await App.Booking.CreateBookingAsync(initialData);
            });

            await StepAsync("Act: Prepare updated data and perform the PUT request", async () =>
            {
                updatedData = BookingDataGenerator.GetBookingFaker().Generate();
                updatedData.firstname = "";
                updatedData.lastname = "";
                booking = await App.Booking.UpdateBookingAsync<BookingRequests, BookingGetResponse>(createdBooking.Data.bookingid, updatedData, AuthToken);
            });

            await StepAsync("Assert: assert the response received is expected", async () =>
            {
                booking.StatusCode.Should().Be(
                    HttpStatusCode.BadRequest,
                    $"the server should return a 400 error for empty data. Received:" +
                    $" {booking.StatusCode} {(int)booking.StatusCode}");
               });
        }

        [Test]
        public async Task UpdateBooking_InvalidTotalPriseFormat_ShouldThrowBadRequest()
        {
            BookingRequests updatedData = null;
            ApiResponse<BookingCreateResponse> createdBooking = null;
            ApiResponse<BookingGetResponseInvalidData> booking = null;

            await StepAsync("Arrange: Cgenerate booking data and create booking", async () =>
            {
                var initialData = BookingDataGenerator.GetBookingFaker().Generate();
                createdBooking = await App.Booking.CreateBookingAsync(initialData);
            });
         
            await StepAsync("Act: Prepare updated data and perform the PUT request", async () =>
            {
                BookingRequestWithWrongDataType updatedData1 = new BookingRequestWithWrongDataType()
                {
                    firstname = "Sally",
                    lastname = "Brown",
                    totalprice = "",
                    depositpaid = true,
                    bookingdates = new BookingDates
                    {
                        checkin = "2017-04-12",
                        checkout = "2021-01-27"
                    },
                    additionalneeds = "Breakfast"
                };
                booking = await App.Booking.UpdateBookingAsync<object, BookingGetResponseInvalidData>(createdBooking.Data.bookingid, updatedData1, AuthToken);
            });

            await StepAsync("Assert: assert the response received is expected", async () =>
            {
                booking.StatusCode.Should().Be(
                        HttpStatusCode.BadRequest,
                        $"the server should return a 400 error for empty data. Received:" +
                        $" {booking.StatusCode} {(int)booking.StatusCode}");
            });
        }

        [Test]
        public async Task UpdateBooking_NonExistentId_ShouldThrowNotFound()
        {
            BookingRequests updatedData = null;
            int firstId = 0;
            ApiResponse<BookingGetResponse> booking = null;

            await StepAsync("Arrange: Get booking ID and generate booking data", async () =>
            {
                var bookingList = await App.Booking.GetAllBookingIdsAsync();
                firstId = bookingList.First().bookingId;
                updatedData = BookingDataGenerator.GetBookingFaker().Generate();
            });
            await StepAsync("Act: Prepare updated data and perform the PUT request", async () =>
            {               
                booking =  await App.Booking.UpdateBookingAsync<BookingRequests, BookingGetResponse>(firstId + 9999999, updatedData, AuthToken);
            });

            await StepAsync("Assert: assert the response received is expected", async () =>
            {
                booking.StatusCode.Should().Be(
                       HttpStatusCode.MethodNotAllowed,
                       $"the server should return a 405 error for empty data. Received:" +
                       $" {booking.StatusCode} {(int)booking.StatusCode}");
            });  
        }
    }
}
