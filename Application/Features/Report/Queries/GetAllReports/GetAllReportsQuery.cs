using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Reports.Queries.GetAllReports
{
  public class GetAllReportsQuery : IRequest<PagedResponse<IEnumerable<GetAllReportsViewModel>>>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }
  public class GetAllReportsQueryHandler : IRequestHandler<GetAllReportsQuery, PagedResponse<IEnumerable<GetAllReportsViewModel>>>
  {
    private readonly IReportRepositoryAsync _reportRepository;
    private readonly IMapper _mapper;
    public GetAllReportsQueryHandler(IReportRepositoryAsync reportRepository,  IMapper mapper)
    {
      _reportRepository = reportRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetAllReportsViewModel>>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
    {
      var validFilter = _mapper.Map<GetAllReportsParameter>(request);
      var reports = await _reportRepository.GetReportsWithRelationsAsync(request.PageNumber, request.PageSize);
      
      var reportViewModels = new List<GetAllReportsViewModel>();
      foreach (var r in reports)
      {
        var eventObj = _mapper.Map<GetAllReportsEventViewModel>(r.Event);
        var student = _mapper.Map<GetAllReportsStudentViewModel>(r.Student);
        var report = _mapper.Map<GetAllReportsViewModel>(r);
        report.ReportedBy = student;
        report.ReportedEvent = eventObj;

        reportViewModels.Add(report);
      }

      return new PagedResponse<IEnumerable<GetAllReportsViewModel>>(reportViewModels, validFilter.PageNumber, validFilter.PageSize);
    }
  }
}
