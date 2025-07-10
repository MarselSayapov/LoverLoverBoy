using Services.Models.Tag.Response;

namespace Services.Models.Task.Response;

public record TicketResponse(Guid Id, string Title, string? Description, string? Status, DateTime? Deadline, Guid? AssignedUserId, Guid ProjectId, IEnumerable<TagResponse> Tags);