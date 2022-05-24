using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities
{
  public class Report : AuditableBaseEntity
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public ReportStates State { get; set; }
  }
}
