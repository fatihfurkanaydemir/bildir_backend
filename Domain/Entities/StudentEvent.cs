using System;
using System.Collections.Generic;
using System.Text;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
  public class StudentEvent
  {
    public int StudentId { get; set; }
    public int EventId { get; set; }
    public Student Student { get; set; }
    public Event Event { get; set; }
    public StudentEventStates State { get; set; }
    public bool? Liked { get; set; }
  }
}
