using Application.Exceptions;
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

namespace Application.Features.Events.Queries.GetEventsByStudentId
{
  public class GetEventsByStudentIdQuery : IRequest<PagedResponse<IEnumerable<GetEventsByStudentIdViewModel>>>
  {
    public int StudentId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }
  public class GetEventsByStudentIdQueryHandler : IRequestHandler<GetEventsByStudentIdQuery, PagedResponse<IEnumerable<GetEventsByStudentIdViewModel>>>
  {
    private readonly IEventRepositoryAsync _eventRepository;
    private readonly IStudentRepositoryAsync _studentRepository;
    private readonly IMapper _mapper;
    public GetEventsByStudentIdQueryHandler(IEventRepositoryAsync eventRepository, IStudentRepositoryAsync studentRepository, IMapper mapper)
    {
      _eventRepository = eventRepository;
      _studentRepository = studentRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetEventsByStudentIdViewModel>>> Handle(GetEventsByStudentIdQuery request, CancellationToken cancellationToken)
    {
      /*
       TODO AUTHENTICATED USER SERVICE
       */
      var student = await _studentRepository.GetByIdAsync(request.StudentId);
      if (student == null) throw new ApiException("Student not found");

      var validFilter = _mapper.Map<GetEventsByStudentIdParameter>(request);
      var events = await _eventRepository.GetEventsByStudentIdWithCommunityAsync(request.StudentId, request.PageNumber, request.PageSize);

      var eventsViewModels = new List<GetEventsByStudentIdViewModel>();

      foreach (var e in events)
      {
        var eventObj = _mapper.Map<GetEventsByStudentIdViewModel>(e);
        var community = _mapper.Map<GetEventsByStudentIdCommunityViewModel>(e.Community);

        eventObj.EventOf = community;

        eventsViewModels.Add(eventObj);
      }

      return new PagedResponse<IEnumerable<GetEventsByStudentIdViewModel>>(eventsViewModels, validFilter.PageNumber, validFilter.PageSize);
    }
  }
}
