namespace Services.Models.Project.Request;

public sealed record CreateProjectRequest(string Name, Guid OwnerId);