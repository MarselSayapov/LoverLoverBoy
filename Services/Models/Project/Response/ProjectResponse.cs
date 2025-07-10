using Services.Models.Task.Response;

namespace Services.Models.Project.Response;

public sealed record ProjectResponse(Guid Id, string Name, Guid OwnerId, IEnumerable<ProjectTicketResponse> Tickets);