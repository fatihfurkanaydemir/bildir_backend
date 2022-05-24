using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Features.Notifications.Queries.GetAllNotificationsByCommunityId
{
  public class GetAllNotificationsByCommunityIdViewModel
  {
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
  }
}
