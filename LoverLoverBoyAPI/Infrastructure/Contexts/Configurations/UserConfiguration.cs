using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(user => user.Id);
        
        builder.Property(user => user.Email)
            .IsRequired();
        builder.Property(user => user.Login)
            .IsRequired();
        builder.Property(user => user.PasswordHash)
            .IsRequired();
    }
}