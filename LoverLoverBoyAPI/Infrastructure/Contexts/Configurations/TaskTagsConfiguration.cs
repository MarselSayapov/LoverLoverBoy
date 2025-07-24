using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Configurations;

public class TaskTagsConfiguration : IEntityTypeConfiguration<TicketTags>
{
    public void Configure(EntityTypeBuilder<TicketTags> builder)
    {
        builder.ToTable("TaskTags");

        builder.HasKey(taskTag => new { taskTag.TaskId, taskTag.TagId });

        builder
            .HasOne(taskTag => taskTag.Ticket)
            .WithMany(task => task.TicketTags)
            .HasForeignKey(taskTag => taskTag.TaskId);

        builder
            .HasOne(taskTag => taskTag.Tag)
            .WithMany(tag => tag.TaskTags)
            .HasForeignKey(taskTag => taskTag.TagId);
    }
}