namespace Services.Models.Task.Request;

public record CreateTicketRequest(string Title, string? Description, string? Status, DateTime? Deadline, Guid? AssignedUserId, Guid ProjectId);