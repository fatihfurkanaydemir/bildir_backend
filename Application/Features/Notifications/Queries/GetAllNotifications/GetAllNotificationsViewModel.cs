using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Features.Notifications.Queries.GetAllNotifications
{
  public class GetAllNotificationsViewModel
  {
    public GetAllNotificationsCommunityViewModel SentBy { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
  }
}
