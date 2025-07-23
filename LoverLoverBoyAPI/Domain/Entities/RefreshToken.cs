namespace Domain.Entities;

public class RefreshToken : EntityBase
{
    public string Token { get; set; }
    public string JwtId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Invalidated { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}