namespace Domain.Entities;

public class TicketTags
{
    public Guid TaskId { get; set; }
    public Ticket Ticket { get; set; }
    public Guid TagId { get; set; }
    public Tag Tag { get; set; }
}