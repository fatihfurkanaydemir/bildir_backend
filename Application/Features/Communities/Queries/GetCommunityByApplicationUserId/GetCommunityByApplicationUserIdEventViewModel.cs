using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Features.Communities.Queries.GetCommunityByApplicationUserId
{
  public class GetCommunityByApplicationUserIdEventViewModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string Tags { get; set; }
    public string State { get; set; }
    public DateTime Date { get; set; }
    public ICollection<Image> Images { get; set; }
  }
}
