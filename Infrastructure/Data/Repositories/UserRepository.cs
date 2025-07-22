using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class UserRepository(ApplicationContext context) : RepositoryBase<User>(context), IUserRepository
{
    private readonly ApplicationContext _context = context;

    public User? GetByEmail(string email)
    {
        return _context.Users.FirstOrDefault(user => user.Email == email);
    }
}