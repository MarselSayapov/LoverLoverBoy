using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Contexts.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(token => token.Token);
        
        builder.Property(token => token.JwtId).IsRequired();
        builder.Property(token => token.ExpiresAt).IsRequired();
        builder.Property(token => token.Invalidated).IsRequired();
        builder.Property(token => token.UserId).IsRequired();
        builder.Property(token => token.JwtId).IsRequired();
        builder.Property(token => token.CreatedAt).IsRequired();
        builder.Property(token => token.UpdatedAt);

        builder
            .HasOne(token => token.User)
            .WithMany()
            .HasForeignKey(token => token.UserId);

    }
}