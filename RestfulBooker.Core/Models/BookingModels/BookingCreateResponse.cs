using Newtonsoft.Json;

namespace RestfulBooker.Core
{
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
}
