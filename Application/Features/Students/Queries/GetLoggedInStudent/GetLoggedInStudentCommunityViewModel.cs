using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Students.Queries.GetLoggedInStudent
{
  public class GetLoggedInStudentCommunityViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public Image Avatar { get; set; }
    public Image BackgroundImage { get; set; }
    public string? InstagramLink { get; set; }
    public string? TwitterLink { get; set; }
    public string? FacebookLink { get; set; }
    public string? LinkedinLink { get; set; }
  }
}
