using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Features.Events.Queries.GetEventById
{
  public class GetEventByIdStudentViewModel
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ApplicationUserId { get; set; }
    public string SchoolEmail { get; set; }
    public string ParticipationState { get; set; }
    public string Faculty { get; set; }
    public string Department { get; set; }
    public string Gender { get; set; }
  }
}
