using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Contexts;

namespace Infrastructure.Data.Repositories;

public class TagRepository(ApplicationContext context) : RepositoryBase<Tag>(context), ITagRepository;