using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeds
{
    public class DefaultNotifications
    {
        public static async Task<bool> SeedAsync(INotificationRepositoryAsync notificationRepository)
        {
            var notification1 = new Notification
            {
                Title = "Example Notification",
                Description = "This is a notification",
                CommunityId = 1,
            };

            var notificationList = await notificationRepository.GetAllAsync();
            var _event1 = notificationList.Where(p => p.Title.StartsWith(notification1.Title)).Count();

            if (_event1 > 0) // ALREADY SEEDED
                return true;

            if (_event1 == 0)
              try
              {
                await notificationRepository.AddAsync(notification1);
              }
              catch (Exception ex)
              {
                Console.WriteLine(ex.Message);
                throw;
              }

            return false; // NOT ALREADY SEEDED

        }
    }
}
