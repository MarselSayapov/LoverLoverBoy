namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IProjectRepository Projects { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    ITagRepository Tags { get; }
    ITicketRepository Tickets { get; }
    Task SaveAsync();
    void Save();
}