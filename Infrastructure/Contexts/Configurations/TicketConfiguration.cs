using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");
        
        builder.HasKey(ticket => ticket.Id);
        
        builder.Property(ticket => ticket.Title).IsRequired();
        builder.Property(ticket => ticket.Description).HasMaxLength(500);
        
        builder
            .HasOne(ticket => ticket.Project)
            .WithMany(project => project.Tickets)
            .HasForeignKey(ticket => ticket.ProjectId);

        builder
            .HasOne(ticket => ticket.AssignedUser)
            .WithMany(user => user.AssignedTickets)
            .HasForeignKey(ticket => ticket.AssignedUserId);
    }
}