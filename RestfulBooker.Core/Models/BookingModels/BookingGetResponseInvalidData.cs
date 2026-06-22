using Newtonsoft.Json;

namespace RestfulBooker.Core
{
    public class BookingGetResponseInvalidData
    {
        [JsonProperty("firstname")]
        public string firstname { get; set; }
        [JsonProperty("lastname")]
        public string lastname { get; set; }
        [JsonProperty("totalprice")]
        public string totalprice { get; set; }
        [JsonProperty("bookingdates")]
        public BookingDates bookingdates { get; set; }
    }
}
