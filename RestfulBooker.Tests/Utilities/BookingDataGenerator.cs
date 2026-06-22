using Bogus;
using RestfulBooker.Core;

public static class BookingDataGenerator
{
    public static Faker<BookingRequests> GetBookingFaker()
    {
        return new Faker<BookingRequests>()
            .RuleFor(u => u.firstname, f => f.Name.FirstName())
            .RuleFor(u => u.lastname, f => f.Name.LastName())
            .RuleFor(u => u.totalprice, f => f.Random.Int(100, 1000))
            .RuleFor(u => u.depositpaid, f => f.Random.Bool())
            .RuleFor(u => u.bookingdates, f => new BookingDates
            {
                checkin = f.Date.Soon(5).ToString("yyyy-MM-dd"),
                checkout = f.Date.Soon(10).ToString("yyyy-MM-dd")
            }).RuleFor(u => u.additionalneeds, f => f.Lorem.Word());
    }
}