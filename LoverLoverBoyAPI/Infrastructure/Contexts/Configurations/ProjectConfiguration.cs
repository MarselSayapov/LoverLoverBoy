using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        
        builder.HasKey(project => project.Id);
        
        builder.Property(project => project.Name).IsRequired();

        builder
            .HasOne(project => project.User)
            .WithMany(user => user.Projects)
            .HasForeignKey(project => project.OwnerId);

    }
}