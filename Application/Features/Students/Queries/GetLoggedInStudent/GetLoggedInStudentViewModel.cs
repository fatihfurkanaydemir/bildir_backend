using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Features.Students.Queries.GetLoggedInStudent
{
  public class GetLoggedInStudentViewModel
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ApplicationUserId { get; set; }
    public string SchoolEmail { get; set; }
    public ICollection<GetLoggedInStudentCommunityViewModel> FollowedCommunities { get; set; }
    public ICollection<GetLoggedInStudentEventViewModel> ParticipatedEvents { get; set; }
    public string Faculty { get; set; }
    public string Department { get; set; }
    public string Gender { get; set; }
  }
}
