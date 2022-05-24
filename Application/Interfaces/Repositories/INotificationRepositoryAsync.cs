using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface INotificationRepositoryAsync : IGenericRepositoryAsync<Notification>
    {
      public Task<IReadOnlyList<Notification>> GetNotificationsWithRelationsAsync(int pageNumber, int pageSize);
      public Task<IReadOnlyList<Notification>> GetNotificationsByCommunityIdAsync(int communityId, int pageNumber, int pageSize);
    }
}
