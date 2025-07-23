using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Contexts;

namespace Infrastructure.Data.Repositories;

public class ProjectRepository(ApplicationContext context) : RepositoryBase<Project>(context), IProjectRepository;