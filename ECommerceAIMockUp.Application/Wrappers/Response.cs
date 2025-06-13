using System.Net;

namespace ECommerceAIMockUp.Application.Wrappers
{
    public class Response<T>
    {
        public Response(T data, HttpStatusCode statusCode, bool isSucceeded)
        {
            Data = data;
            StatusCode = statusCode;
            IsSucceeded = isSucceeded;
        }

        public Response(T data, HttpStatusCode statusCode, string message, bool isSucceeded)
        {
            Data = data;
            StatusCode = statusCode;
            Message = message;
            IsSucceeded = isSucceeded;
        }

        public T Data { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsSucceeded { get; set; }

    }
}
