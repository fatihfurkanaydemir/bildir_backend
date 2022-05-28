using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Features.Students.Queries.GetStudentByApplicationUserId
{
  public class GetStudentByApplicationUserIdViewModel
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ApplicationUserId { get; set; }
    public string SchoolEmail { get; set; }
    public ICollection<GetStudentByApplicationUserIdCommunityViewModel> FollowedCommunities { get; set; }
    public ICollection<GetStudentByApplicationUserIdEventViewModel> ParticipatedEvents { get; set; }
    public string Faculty { get; set; }
    public string Department { get; set; }
    public string Gender { get; set; }
  }
}
