using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
  public interface IStudentEventRepositoryAsync : IGenericRepositoryAsync<StudentEvent>
  {
    public Task<StudentEvent> GetStudentEventByCompositePKAsync(int studentId, int eventId);
    public Task<IEnumerable<StudentEvent>> GetStudentEventsByEventIdAsync(int eventId);
  }
}
