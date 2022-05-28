using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Features.Communities.Queries.GetAllCommunities
{
  public class GetAllCommunitiesViewModel
  {
    public int Id { get; set; }
    public string CreationKey { get; set; }
    public string ApplicationUserId { get; set; }
    public bool IsKeyUsed { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public ICollection<GetAllCommunitiesStudentViewModel> Followers { get; set; }
    public ICollection<GetAllCommunitiesEventViewModel> OrganizedEvents { get; set; }
    public Image Avatar { get; set; }
    public Image BackgroundImage { get; set; }
    public string? InstagramLink { get; set; }
    public string? TwitterLink { get; set; }
    public string? FacebookLink { get; set; }
    public string? LinkedinLink { get; set; }
  }
}
