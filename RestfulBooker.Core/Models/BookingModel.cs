using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace RestfulBooker.Core
{
    public class BookingModel
    {
        public class BookingRequest
        {
            [JsonProperty("firstname")]
            public string firstname { get; set; }
            [JsonProperty("lastname")]
            public string lastname { get; set; }
            [JsonProperty("totalprice")]
            public int totalprice { get; set; }
            [JsonProperty("depositpaid")]
            public bool depositpaid { get; set; }
            [JsonProperty("bookingdates")]
            public BookingDates bookingdates { get; set; }
            [JsonProperty("additionalneeds")]
            public string additionalneeds { get; set; }
        }

        public class BookingDates
        {
            [JsonProperty("checkin")]
            public string checkin { get; set; }
            [JsonProperty("checkout")]
            public string checkout { get; set; }
        }

        // This maps the response JSON to a C# object
        public class BookingGetResponse
        {
            [JsonProperty("firstname")]
            public string firstname { get; set; }
            [JsonProperty("lastname")]
            public string lastname { get; set; }
            [JsonProperty("totalprice")]
            public int totalprice { get; set; }
            [JsonProperty("bookingdates")]
            public BookingDates bookingdates { get; set; }
        }

        public class BookingCreateResponse
        {
            [JsonProperty("bookingid")]
            public int bookingid { get; set; }
            public BookingData booking { get; set; }
        }

        public class BookingData
        {
            [JsonProperty("firstname")]
            public string firstname { get; set; }

            [JsonProperty("lastname")]
            public string lastname { get; set; }

            [JsonProperty("totalprice")]
            public int totalprice { get; set; }

            [JsonProperty("depositpaid")]
            public bool depositpaid { get; set; }

            [JsonProperty("bookingdates")]
            public BookingDates bookingdates { get; set; }

            [JsonProperty("additionalneeds")]
            public string additionalneeds { get; set; }
        }

        public class BookingSummary
        {
            [JsonPropertyName("bookingid")]
            public int bookingId { get; set; }
        }
    }
}
