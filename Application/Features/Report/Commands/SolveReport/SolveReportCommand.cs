using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Reports.Commands.SolveReport
{
  public class SolveReportCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
  }
  public class SolveReportCommandHandler : IRequestHandler<SolveReportCommand, Response<int>>
  {
    private readonly IReportRepositoryAsync _reportRepository;
    private readonly IMapper _mapper;
    public SolveReportCommandHandler(IReportRepositoryAsync reportRepository, IMapper mapper)
    {
      _reportRepository = reportRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(SolveReportCommand request, CancellationToken cancellationToken)
    {
      var report = await _reportRepository.GetByIdAsync(request.Id);
      if (report == null) throw new ApiException("Report not found");
      
      report.State = ReportStates.Solved;

      await _reportRepository.UpdateAsync(report);
      return new Response<int>(report.Id);
    }
  }
}
