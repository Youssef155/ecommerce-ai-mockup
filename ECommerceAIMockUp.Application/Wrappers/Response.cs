using System.Net;

namespace ECommerceAIMockUp.Application.Wrappers
{
    public class Response<T>
    {
        public Response() { }
        public Response(T data, HttpStatusCode statusCode, bool isSucceeded)
        {
            Data = data;
            StatusCode = statusCode;
            IsSucceeded = isSucceeded;
        }

        public T Data { get; set; }
        public string Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSucceeded { get; set; }

    }
}
