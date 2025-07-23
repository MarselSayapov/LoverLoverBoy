namespace Services.Models.Task.Request;

public sealed record PatchTicketRequest(DateTime? Deadline, Guid? AssigneeId);