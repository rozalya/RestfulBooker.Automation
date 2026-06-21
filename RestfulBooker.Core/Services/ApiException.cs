using System.Net;

namespace RestfulBooker.Core
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public ApiException(HttpStatusCode statusCode, string message) : base(message)
            => StatusCode = statusCode;
    }

    public class ResourceNotFoundException : ApiException
    {
        public ResourceNotFoundException(string message) : base(HttpStatusCode.NotFound, message) { }
    }
}
