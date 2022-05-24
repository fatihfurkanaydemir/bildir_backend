using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
  public class StudentCommunity
  {
    public int StudentId { get; set; }
    public int CommunityId { get; set; }
    public Student Student { get; set; }
    public Community Community { get; set; }
  }
}
