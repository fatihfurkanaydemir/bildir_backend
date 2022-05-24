using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities
{
  public class Notification : AuditableBaseEntity
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public Community Community { get; set; }
    public int CommunityId { get; set; }
  }
}
