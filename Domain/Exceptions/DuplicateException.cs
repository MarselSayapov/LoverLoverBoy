using System.Net;

namespace Domain.Exceptions;

public class DuplicateException(string message) : BaseException(message, HttpStatusCode.Conflict);