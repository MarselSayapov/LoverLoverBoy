namespace Domain.Entities;

public class Ticket : EntityBase
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public DateTime? Deadline { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public Guid? AssignedUserId { get; set; }
    public User? AssignedUser { get; set; }
    public ICollection<TicketTags>  TicketTags { get; set; } = new  List<TicketTags>();
}