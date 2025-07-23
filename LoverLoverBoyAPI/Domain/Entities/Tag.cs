namespace Domain.Entities;

public class Tag : EntityBase
{
    public string Name { get; set; }
    public ICollection<TicketTags> TaskTags { get; set; } = new List<TicketTags>();
}