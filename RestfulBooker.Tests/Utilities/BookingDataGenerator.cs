using Bogus;
using static RestfulBooker.Core.BookingModel;

public static class BookingDataGenerator
{
    public static Faker<BookingRequest> GetBookingFaker()
    {
        return new Faker<BookingRequest>()
            .RuleFor(u => u.firstname, f => f.Name.FirstName())
            .RuleFor(u => u.lastname, f => f.Name.LastName())
            .RuleFor(u => u.totalprice, f => f.Random.Int(100, 1000))
            .RuleFor(u => u.depositpaid, f => f.Random.Bool())
            .RuleFor(u => u.bookingdates, f => new BookingDates
            {
                checkin = f.Date.Soon(5).ToString("yyyy-MM-dd"),
                checkout = f.Date.Soon(10).ToString("yyyy-MM-dd")
            });
    }
}