using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Contexts;

namespace Infrastructure.Data.Repositories;

public class TicketRepository(ApplicationContext context) : RepositoryBase<Ticket>(context), ITicketRepository;