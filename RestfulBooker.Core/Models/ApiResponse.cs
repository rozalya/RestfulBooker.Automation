using System.Net;

namespace RestfulBooker.Core
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
