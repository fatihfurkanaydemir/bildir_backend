using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Students.Queries.GetStudentById
{
  public class GetStudentByIdEventViewModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string Tags { get; set; }
    public GetStudentByIdEventViewModel EventOf { get; set; }
    public EventStates State { get; set; }
    public DateTime Date { get; set; }
  }
}
