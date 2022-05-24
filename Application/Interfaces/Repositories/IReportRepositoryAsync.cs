using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IReportRepositoryAsync : IGenericRepositoryAsync<Report>
    {
      public Task<IReadOnlyList<Report>> GetReportsWithRelationsAsync(int pageNumber, int pageSize);
      public Task<Report> GetReportByStudentAndEventIdAsync(int studentId, int eventId);
    }
}
