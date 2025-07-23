using Services.Models.Tag.Response;

namespace Services.Models.Task.Response;

public sealed record ProjectTicketResponse(string Title, string? Description, string? Status, DateTime? Deadline, Guid? AssignedUserId, IEnumerable<TagResponse> Tags);