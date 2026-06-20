using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace RestfulBooker.Core
{
    public class BookingModel
    {
        public class BookingRequest
        {
            public string firstname { get; set; }
            public string lastname { get; set; }
            public int totalprice { get; set; }
            public bool depositpaid { get; set; }
            public BookingDates bookingdates { get; set; }
            public string additionalneeds { get; set; }
        }

        public class BookingDates
        {
            public string checkin { get; set; }
            public string checkout { get; set; }
        }

        // This maps the response JSON to a C# object
        public class BookingResponse
        {
            [JsonProperty("bookingid")]
            public int bookingid { get; set; }
            public BookingRequest booking { get; set; }
        }
        public class BookingSummary
        {
            [JsonPropertyName("bookingid")]
            public int bookingId { get; set; }
        }
    }
}
