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
    public class ReportRepositoryAsync : GenericRepositoryAsync<Report>, IReportRepositoryAsync
    {
        private readonly DbSet<Report> _reports;

        public ReportRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
          _reports = dbContext.Set<Report>();
        }

        public async Task<IReadOnlyList<Report>> GetReportsWithRelationsAsync(int pageNumber, int pageSize)
        {
            return await _reports
                .Include(r => r.Student)
                .Include(r => r.Event)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Report> GetReportByStudentAndEventIdAsync(int studentId, int eventId)
        {
            return await _reports
                .SingleOrDefaultAsync(r => r.StudentId == studentId && r.EventId == eventId);
        }
    }
}
