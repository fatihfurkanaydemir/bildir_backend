using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Account
{
  public class CommunityCreateRequest
  {
    [Required]
    public string Name { get; set; }
  }
}
