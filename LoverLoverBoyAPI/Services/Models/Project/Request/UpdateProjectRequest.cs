namespace Services.Models.Project.Request;

public sealed record UpdateProjectRequest(string Name, Guid OwnerId);