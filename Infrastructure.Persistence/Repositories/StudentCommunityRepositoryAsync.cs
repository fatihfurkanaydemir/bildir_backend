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
  public class StudentCommunityRepositoryAsync : GenericRepositoryAsync<StudentCommunity>, IStudentCommunityRepositoryAsync
  {
    private readonly DbSet<StudentCommunity> _studentCommunities;
    public StudentCommunityRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
      _studentCommunities = dbContext.Set<StudentCommunity>();
    }

    public async Task<StudentCommunity> GetStudentCommunityByCompositePKAsync(int studentId, int communityId)
    {
      return await _studentCommunities
        .SingleOrDefaultAsync(x => x.StudentId == studentId && x.CommunityId == communityId);
    }
  }
}