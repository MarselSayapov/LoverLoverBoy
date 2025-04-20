using System.Net;

namespace Domain.Exceptions;

public class BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    : Exception(message)
{
    public HttpStatusCode StatusCode { get; set; } = statusCode;
}