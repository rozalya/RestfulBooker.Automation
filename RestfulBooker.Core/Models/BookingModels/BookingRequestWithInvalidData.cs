using Newtonsoft.Json;

namespace RestfulBooker.Core
{
    public class BookingRequestWithWrongDataType
    {
        [JsonProperty("firstname")]
        public string firstname { get; set; }
        [JsonProperty("lastname")]
        public string lastname { get; set; }
        [JsonProperty("totalprice")]
        public string totalprice { get; set; }
        [JsonProperty("depositpaid")]
        public bool depositpaid { get; set; }
        [JsonProperty("bookingdates")]
        public BookingDates bookingdates { get; set; }
        [JsonProperty("additionalneeds")]
        public string additionalneeds { get; set; }
    }
}
