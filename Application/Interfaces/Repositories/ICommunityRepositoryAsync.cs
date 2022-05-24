using Application.Features.Communities.Queries.GetAllCommunities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Application.Interfaces.Repositories
{
  public interface ICommunityRepositoryAsync : IGenericRepositoryAsync<Community>
  {
    public Task<Community> GetCommunityByCreationKeyAsync(string creationKey);
    public Task<Community> GetCommunityByApplicationUserIdAsync(string applicationUserId);
    public Task<Community> GetCommunityByIdWithRelationsAsync(int id);
    public Task<IReadOnlyList<Community>> GetCommunitiesWithRelationsAsync(int pageNumber, int pageSize);
    public Task<Community> GetCommunityByApplicationUserIdWithRelationsAsync(string applicationUserId);

  }
}
