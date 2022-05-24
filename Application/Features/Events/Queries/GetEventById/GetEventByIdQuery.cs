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

namespace Application.Features.Events.Queries.GetEventById
{
  public class GetEventByIdQuery : IRequest<Response<GetEventByIdViewModel>>
  {
    public int Id { get; set; }
  }
  public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Response<GetEventByIdViewModel>>
  {
    private readonly IEventRepositoryAsync _eventRepository;
    private readonly IMapper _mapper;
    public GetEventByIdQueryHandler(IEventRepositoryAsync eventRepository, IMapper mapper)
    {
      _eventRepository = eventRepository;
      _mapper = mapper;
    }

    public async Task<Response<GetEventByIdViewModel>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
      var eventObj = await _eventRepository.GetEventByIdWithRelationsAsync(request.Id);
      if (eventObj == null) throw new ApiException("Event not found");

      var eventViewModel = _mapper.Map<GetEventByIdViewModel>(eventObj);
      var community = _mapper.Map<GetEventByIdCommunityViewModel>(eventObj.Community);
      var participants = new List<GetEventByIdStudentViewModel>();

      foreach (var s in eventObj.Students)
        participants.Add(_mapper.Map<GetEventByIdStudentViewModel>(s.Student));

      eventViewModel.EventOf = community;
      eventViewModel.Participants = participants;

      return new Response<GetEventByIdViewModel>(eventViewModel);
    }
  }
}
