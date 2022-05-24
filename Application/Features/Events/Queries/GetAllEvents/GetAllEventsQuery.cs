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

namespace Application.Features.Events.Queries.GetAllEvents
{
  public class GetAllEventsQuery : IRequest<PagedResponse<IEnumerable<GetAllEventsViewModel>>>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }
  public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, PagedResponse<IEnumerable<GetAllEventsViewModel>>>
  {
    private readonly IEventRepositoryAsync _eventRepository;
    private readonly IMapper _mapper;
    public GetAllEventsQueryHandler(IEventRepositoryAsync eventRepository, IMapper mapper)
    {
      _eventRepository = eventRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetAllEventsViewModel>>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
      var validFilter = _mapper.Map<GetAllEventsParameter>(request);
      var events = await _eventRepository.GetEventsWithRelationsAsync(request.PageNumber, request.PageSize);

      var eventsViewModels = new List<GetAllEventsViewModel>();

      foreach (var e in events)
      {
        var eventObj = _mapper.Map<GetAllEventsViewModel>(e);
        var community = _mapper.Map<GetAllEventsCommunityViewModel>(e.Community);
        var participants = new List<GetAllEventsStudentViewModel>();

        foreach (var s in e.Students)
          participants.Add(_mapper.Map<GetAllEventsStudentViewModel>(s.Student));

        eventObj.EventOf = community;
        eventObj.Participants = participants;

        eventsViewModels.Add(eventObj);
      }

      return new PagedResponse<IEnumerable<GetAllEventsViewModel>>(eventsViewModels, validFilter.PageNumber, validFilter.PageSize);
    }
  }
}
