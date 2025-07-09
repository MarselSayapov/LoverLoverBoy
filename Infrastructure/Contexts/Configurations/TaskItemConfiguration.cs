using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");
        
        builder.HasKey(task => task.Id);
        
        builder.Property(task => task.Title).IsRequired();
        builder.Property(task => task.Description).HasMaxLength(500);
        
        builder
            .HasOne(task => task.Project)
            .WithMany(project => project.Tasks)
            .HasForeignKey(task => task.ProjectId);

        builder
            .HasOne(task => task.AssignedUser)
            .WithMany(user => user.AssignedTasks)
            .HasForeignKey(task => task.AssignedUserId);
    }
}