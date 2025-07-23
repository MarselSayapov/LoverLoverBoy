namespace Services.Models.Task.Request;

public sealed record UpdateTicketRequest(Guid ProjectId, string Title, string Description, string Status, DateTime Deadline, Guid AssignedUserId);