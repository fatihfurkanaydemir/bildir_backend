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
  public class StudentRepositoryAsync : GenericRepositoryAsync<Student>, IStudentRepositoryAsync
  {
    private readonly DbSet<Student> _students;
    public StudentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
      _students = dbContext.Set<Student>();
    }

    public async Task<Student> GetStudentByApplicationUserIdAsync(string applicationUserId)
    {
      return await _students
        .SingleOrDefaultAsync(x => x.ApplicationUserId == applicationUserId);
    }

    public async Task<Student> GetStudentByIdWithRelationsAsync(int id)
    {
      return await _students
        .Include(s => s.Communities)
        .ThenInclude(sc => sc.Community)
        .Include(s => s.Events)
        .ThenInclude(se => se.Event)
        .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Student> GetStudentByApplicationUserIdWithRelationsAsync(string applicationUserId)
    {
      return await _students
        .Include(x => x.Communities)
        .ThenInclude(sc => sc.Community)
        .Include(s => s.Events)
        .ThenInclude(se => se.Event)
        .SingleOrDefaultAsync(s => s.ApplicationUserId == applicationUserId);
    }

    public async Task<IReadOnlyList<Student>> GetStudentsWithRelationsAsync(int pageNumber, int pageSize)
    {
      return await _students
        .Include(s => s.Communities)
        .ThenInclude(sc => sc.Community)
        .Include(s => s.Events)
        .ThenInclude(se => se.Event)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();
    }
  }
}
