using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeds
{
    public class DefaultEvents
    {
        public static async Task<bool> SeedAsync(IEventRepositoryAsync eventRepository)
        {
            var event1 = new Event
            {
                Title = "Example Event",
                Description = "This Event will be awesome",
                Location = "Antalya",
                CommunityId = 1,
                Tags = "event,antalya,bildir",
                Date = new DateTime(2022, 8, 5),
                State = Domain.Enums.EventStates.Active,
            };

            var eventList = await eventRepository.GetAllAsync();
            var _event1 = eventList.Where(p => p.Title.StartsWith(event1.Title)).Count();

            if (_event1 > 0) // ALREADY SEEDED
                return true;

            if (_event1 == 0)
              try
              {
                await eventRepository.AddAsync(event1);
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
