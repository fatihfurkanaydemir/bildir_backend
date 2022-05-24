using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities
{
  public class Event : AuditableBaseEntity
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string Tags { get; set; }
    public ICollection<StudentEvent> Students { get; set; }
    public ICollection<Image> Images { get; set; }
    public Community Community { get; set; }
    public int CommunityId { get; set; }
    public EventStates State { get; set; }
    public DateTime Date { get; set; }
  }
}
