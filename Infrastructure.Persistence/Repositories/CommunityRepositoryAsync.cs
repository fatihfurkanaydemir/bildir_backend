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
  public class CommunityRepositoryAsync : GenericRepositoryAsync<Community>, ICommunityRepositoryAsync
  {
    private readonly DbSet<Community> _communities;
    public CommunityRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
      _communities = dbContext.Set<Community>();
    }

    public async Task<Community> GetCommunityByCreationKeyAsync(string creationKey)
    {
      return await _communities
        .SingleOrDefaultAsync(x => x.CreationKey == creationKey);
    }

    public async Task<Community> GetCommunityByApplicationUserIdAsync(string applicationUserId)
    {
      return await _communities
        .SingleOrDefaultAsync(x => x.ApplicationUserId == applicationUserId);
    }

    public async Task<Community> GetCommunityByApplicationUserIdWithRelationsAsync(string applicationUserId)
    {
      return await _communities
        .Include(c => c.Students)
        .ThenInclude(sc => sc.Student)
        .Include(c => c.Avatar)
        .Include(c => c.BackgroundImage)
        .Include(c => c.Events)
        .SingleOrDefaultAsync(c => c.ApplicationUserId == applicationUserId);
    }

    public async Task<IReadOnlyList<Community>> GetCommunitiesWithRelationsAsync(int pageNumber, int pageSize)
    {
      return await _communities
        .Include(c => c.Students)
        .ThenInclude(sc => sc.Student)
        .Include(c => c.Avatar)
        .Include(c => c.BackgroundImage)
        .Include(c => c.Events)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<Community> GetCommunityByIdWithRelationsAsync(int id)
    {
      return await _communities
        .Include(c => c.Students)
        .ThenInclude(sc => sc.Student)
        .Include(c => c.Events)
        .Include(c => c.Avatar)
        .Include(c => c.BackgroundImage)
        .SingleOrDefaultAsync(c => c.Id == id);
    }
  }
}
