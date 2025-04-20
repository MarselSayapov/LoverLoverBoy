using System.Net;

namespace Domain.Exceptions;

public class NotFoundException(string message) : BaseException(message, HttpStatusCode.NotFound);