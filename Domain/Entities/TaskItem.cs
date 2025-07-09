using Domain.Enum;

namespace Domain.Entities;

public class TaskItem : EntityBase
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;
    public DateTime? Deadline { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public Guid? AssignedUserId { get; set; }
    public User? AssignedUser { get; set; }
    public ICollection<TaskTagItem>  TaskTags { get; set; } = new  List<TaskTagItem>();
}