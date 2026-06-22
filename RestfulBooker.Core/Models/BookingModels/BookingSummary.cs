using System.Text.Json.Serialization;

namespace RestfulBooker.Core
{
    public class BookingSummary
    {
        [JsonPropertyName("bookingid")]
        public int bookingId { get; set; }
    }
}
