namespace Domain.Entities;

public class Tag : EntityBase
{
    public string Name { get; set; }
    public ICollection<TaskTagItem> TaskTags { get; set; } = new  List<TaskTagItem>();
}