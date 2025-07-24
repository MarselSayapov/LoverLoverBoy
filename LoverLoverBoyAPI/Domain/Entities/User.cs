namespace Domain.Entities;

public class User : EntityBase
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();

}