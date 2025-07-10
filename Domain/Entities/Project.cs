namespace Domain.Entities;

public class Project : EntityBase
{
    public string Name { get; set; }
    public Guid OwnerId { get; set; }
    public User User { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = new  List<Ticket>();
}