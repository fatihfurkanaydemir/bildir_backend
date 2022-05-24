using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
  public class Community: AuditableBaseEntity
  {
    public string CreationKey { get; set; }
    public bool IsKeyUsed { get; set; }
    public string Name { get; set; }
    public string ApplicationUserId { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public ICollection<StudentCommunity> Students { get; set; }
    public ICollection<Event> Events { get; set; }
    public Image Avatar { get; set; }
    public Image BackgroundImage { get; set; }
    public string? InstagramLink { get; set; }
    public string? TwitterLink { get; set; }
    public string? FacebookLink { get; set; }
    public string? LinkedinLink { get; set; }
  }
}
