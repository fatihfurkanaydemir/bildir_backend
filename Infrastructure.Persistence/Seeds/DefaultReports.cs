using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeds
{
    public class DefaultReports
    {
        public static async Task<bool> SeedAsync(IReportRepositoryAsync reportRepository)
        {
            var report1 = new Report
            {
                Title = "Example Report",
                Description = "This event has bad words in it's description !!!",
                StudentId = 2,
                EventId = 1,
            };

            var reportList = await reportRepository.GetAllAsync();
            var _event1 = reportList.Where(p => p.Title.StartsWith(report1.Title)).Count();

            if (_event1 > 0) // ALREADY SEEDED
                return true;

            if (_event1 == 0)
              try
              {
                await reportRepository.AddAsync(report1);
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
