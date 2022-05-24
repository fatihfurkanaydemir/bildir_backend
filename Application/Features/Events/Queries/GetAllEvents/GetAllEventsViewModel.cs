using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Events.Queries.GetAllEvents
{
  public class GetAllEventsViewModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string Tags { get; set; }
    public GetAllEventsCommunityViewModel EventOf { get; set; }
    public ICollection<GetAllEventsStudentViewModel> Participants { get; set; }
    public ICollection<Image> Images { get; set; }
    public string State { get; set; }
    public DateTime Date { get; set; }
  }
}
