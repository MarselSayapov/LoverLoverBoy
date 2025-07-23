using Domain.Interfaces;
using Infrastructure.Contexts;
using Infrastructure.Data.Repositories;

namespace Infrastructure.Data.UnitOfWork;

public class UnitOfWork(ApplicationContext context) : IUnitOfWork
{
    private UserRepository? _userRepository;
    private TicketRepository? _ticketRepository;
    private IRefreshTokenRepository? _refreshTokenRepository;
    private ITagRepository? _tagRepository;
    private IProjectRepository? _projectRepository;
    
    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }

    public IUserRepository Users
    {
        get
        {
            return _userRepository ??= new UserRepository(context);
        }
    }

    public IProjectRepository Projects
    {
        get
        {
            return _projectRepository ??= new ProjectRepository(context);
        }
    }

    public IRefreshTokenRepository RefreshTokens
    {
        get
        {
            return _refreshTokenRepository ??= new RefreshTokenRepository(context);
        }
    }

    public ITagRepository Tags
    {
        get
        {
            return _tagRepository ??= new TagRepository(context);
        }
    }

    public ITicketRepository Tickets
    {
        get
        {
            return _ticketRepository ??= new TicketRepository(context);
        }
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public void Save()
    {
        context.SaveChanges();
    }
}