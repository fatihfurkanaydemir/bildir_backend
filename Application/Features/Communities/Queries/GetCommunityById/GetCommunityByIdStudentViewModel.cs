using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Communities.Queries.GetCommunityById
{
  public class GetCommunityByIdStudentViewModel
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ApplicationUserId { get; set; }
    public string SchoolEmail { get; set; }
    public string Faculty { get; set; }
    public string Department { get; set; }
    public string Gender { get; set; }
  }
}
