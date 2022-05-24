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
    public class NotificationRepositoryAsync : GenericRepositoryAsync<Notification>, INotificationRepositoryAsync
    {
        private readonly DbSet<Notification> _notifications;

        public NotificationRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
          _notifications = dbContext.Set<Notification>();
        }

        public async Task<IReadOnlyList<Notification>> GetNotificationsByCommunityIdAsync(int communityId, int pageNumber, int pageSize)
        {
            return await _notifications
                .Where(n => n.CommunityId == communityId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Notification>> GetNotificationsWithRelationsAsync(int pageNumber, int pageSize)
        {
            return await _notifications
                .Include(e => e.Community)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
