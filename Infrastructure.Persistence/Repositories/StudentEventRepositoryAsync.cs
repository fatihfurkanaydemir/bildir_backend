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
using AutoMapper;

namespace Infrastructure.Persistence.Repositories
{
  public class StudentEventRepositoryAsync : GenericRepositoryAsync<StudentEvent>, IStudentEventRepositoryAsync
  {
    private readonly DbSet<StudentEvent> _studentEvents;
    public StudentEventRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
      _studentEvents = dbContext.Set<StudentEvent>();
    }

    public async Task<StudentEvent> GetStudentEventByCompositePKAsync(int studentId, int eventId)
    {
      return await _studentEvents
        .SingleOrDefaultAsync(x => x.StudentId == studentId && x.EventId == eventId);
    }

    public async Task<IEnumerable<StudentEvent>> GetStudentEventsByEventIdAsync(int eventId)
    {
      return await _studentEvents
        .Where(se => se.EventId == eventId)
        .AsTracking()
        .ToListAsync();
    }

    //public async Task<EventParticipation> GetEventParticipationByApplicationUserIdAsync(string applicationUserId)
    //{
    //  return await _students
    //    .SingleOrDefaultAsync(x => x.ApplicationUserId == applicationUserId);
    //}

    //public async Task<EventParticipation> GetEventParticipationByIdWithRelationsAsync(int id)
    //{
    //  return await _students
    //    .Include(s => s.FollowedCommunities)
    //    .ThenInclude(sc => sc.Community)
    //    .SingleOrDefaultAsync(x => x.Id == id);
    //}

    //public async Task<EventParticipation> GetEventParticipationByApplicationUserIdWithRelationsAsync(string applicationUserId)
    //{
    //  return await _students
    //    .Include(x => x.FollowedCommunities)
    //    .ThenInclude(sc => sc.Community)
    //    .SingleOrDefaultAsync(s => s.ApplicationUserId == applicationUserId);
    //}

    //public async Task<IReadOnlyList<EventParticipation>> GetEventParticipationsWithRelationsAsync(int pageNumber, int pageSize)
    //{
    //  return await _students
    //      .Include(s => s.FollowedCommunities)
    //      .ThenInclude(sc => sc.Community)
    //      .Skip((pageNumber - 1) * pageSize)
    //      .Take(pageSize)
    //      .AsNoTracking()
    //      .ToListAsync();
    //}
  }
}
