using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class EventRepositoryAsync : GenericRepositoryAsync<Event>, IEventRepositoryAsync
    {
        private readonly DbSet<Event> _events;

        public EventRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _events = dbContext.Set<Event>();
        }

        public async Task<Event> GetEventByIdWithRelationsAsync(int eventId)
        {
            return await _events
                .Include(e => e.Community)
                .Include(e => e.Students)
                .ThenInclude(se => se.Student)
                .Include(e => e.Images)
                .SingleOrDefaultAsync(x => x.Id == eventId);
        }

        public async Task<IReadOnlyList<Event>> GetEventsWithRelationsAsync(int pageNumber, int pageSize)
        {
            return await _events
                .Include(e => e.Community)
                .Include(e => e.Students)
                .ThenInclude(se => se.Student)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Event>> GetEventsByStudentIdWithCommunityAsync(int studentId, int pageNumber, int pageSize)
        {
            return await _events
                .Where(e => e.Students.Any(se => se.StudentId == studentId))
                .Include(e => e.Community)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Event> GetEventByIdWithCommunityAsync(int eventId) 
        { 
            return await _events
              .Include(e => e.Community)
              .Include(e => e.Images)
              .SingleOrDefaultAsync(e => e.Id == eventId);
        }
    }
}
