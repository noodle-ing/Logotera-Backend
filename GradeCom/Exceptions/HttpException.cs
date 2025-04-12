using System.Net;

namespace GradeCom.Exceptions
{
    public class HttpException : ApplicationException
    {
        public int StatusCode { get; }

        public HttpException(int statusCode, string? message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpException(HttpStatusCode statusCode, string? message) : base(message)
        {
            StatusCode = (int)statusCode;
        }
    }
}
