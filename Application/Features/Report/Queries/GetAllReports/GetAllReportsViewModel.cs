using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Features.Reports.Queries.GetAllReports
{
  public class GetAllReportsViewModel
  {
    public GetAllReportsStudentViewModel ReportedBy { get; set; }
    public GetAllReportsEventViewModel ReportedEvent { get; set; }
    public string Title { get; set; }
    public string State { get; set; }
    public string Description { get; set; }
  }
}
