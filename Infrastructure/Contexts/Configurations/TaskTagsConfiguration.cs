using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Configurations;

public class TaskTagsConfiguration : IEntityTypeConfiguration<TaskTagItem>
{
    public void Configure(EntityTypeBuilder<TaskTagItem> builder)
    {
        builder.ToTable("TaskTags");
        
        builder.HasKey(taskTag => new { taskTag.TaskId, taskTag.TagId });

        builder
            .HasOne(taskTag => taskTag.Task)
            .WithMany(task => task.TaskTags)
            .HasForeignKey(taskTag => taskTag.TaskId);
        
        builder
            .HasOne(taskTag => taskTag.Tag)
            .WithMany(tag => tag.TaskTags)
            .HasForeignKey(taskTag => taskTag.TagId);
    }
}