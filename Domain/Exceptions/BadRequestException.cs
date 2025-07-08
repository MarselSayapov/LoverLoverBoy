using System.Net;

namespace Domain.Exceptions;

public class BadRequestException(string message) : BaseException(message, HttpStatusCode.BadRequest);