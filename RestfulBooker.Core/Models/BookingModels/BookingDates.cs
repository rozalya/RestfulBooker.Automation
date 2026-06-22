using Newtonsoft.Json;

namespace RestfulBooker.Core
{
    public class BookingDates
    {
        [JsonProperty("checkin")]
        public string checkin { get; set; }
        [JsonProperty("checkout")]
        public string checkout { get; set; }
    }
}
