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

namespace Application.Features.Reports.Commands.CreateReport
{
  public class CreateReportCommand : IRequest<Response<int>>
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public int StudentId { get; set; }
    public int EventId { get; set; }
  }
  public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Response<int>>
  {
    private readonly IReportRepositoryAsync _reportRepository;
    private readonly IStudentRepositoryAsync _studentRepository;
    private readonly IEventRepositoryAsync _eventRepository;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IMapper _mapper;
    public CreateReportCommandHandler(IReportRepositoryAsync reportRepository, IStudentRepositoryAsync studentRepository, IEventRepositoryAsync eventRepository, IAuthenticatedUserService authenticatedUserService, IMapper mapper)
    {
      _reportRepository = reportRepository;
      _studentRepository = studentRepository;
      _eventRepository = eventRepository;
      _authenticatedUserService = authenticatedUserService;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
      /*TODO*/
      //if (_authenticatedUserService.UserId == null) throw new ApiException("User not logged in");
      //var student = await _studentRepository.GetStudentByApplicationUserIdAsync(_authenticatedUserService.UserId);
      var student = await _studentRepository.GetByIdAsync(request.StudentId);
      if (student == null) throw new ApiException("Student not found");

      var eventObj = await _eventRepository.GetByIdAsync(request.EventId);
      if (eventObj == null) throw new ApiException("Event not found");

      var existingReport = await _reportRepository.GetReportByStudentAndEventIdAsync(request.StudentId, request.EventId);
      if (existingReport != null) throw new ApiException("This student has already reported this event");

      var report = _mapper.Map<Report>(request);
      report.State = ReportStates.Waiting;

      await _reportRepository.AddAsync(report);
      return new Response<int>(report.Id);
    }
  }
}
