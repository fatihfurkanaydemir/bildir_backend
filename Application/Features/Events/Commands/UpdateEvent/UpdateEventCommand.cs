using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Events.Commands.UpdateEvent
{
  public class UpdateEventCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
    public int CommunityId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string Tags { get; set; }
    public DateTime Date { get; set; }

    public class UpdatePersonnelCommandHandler : IRequestHandler<UpdateEventCommand, Response<int>>
    {
      private readonly IEventRepositoryAsync _eventRepository;
      private readonly IAuthenticatedUserService _authenticatedUserService;

      private readonly IMapper _mapper;
      public UpdatePersonnelCommandHandler(IEventRepositoryAsync eventRepository, IAuthenticatedUserService authenticatedUserService, IMapper mapper)
      {
        _eventRepository = eventRepository;
        _authenticatedUserService = authenticatedUserService;
        _mapper = mapper;
      }
      public async Task<Response<int>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
      {
        //if (_authenticatedUserService.UserId == null) throw new ApiException("User not logged in");

        var eventObj = await _eventRepository.GetEventByIdWithCommunityAsync(request.Id);
        if (eventObj == null) throw new ApiException("Event not found");

        if (eventObj.State == EventStates.Canceled) throw new ApiException("This event is canceled");
        if (eventObj.State == EventStates.Ended) throw new ApiException("This event is already ended");
        //if (eventObj.Community.ApplicationUserId != _authenticatedUserService.UserId) throw new ApiException("This event doesn't belong to this community");
        if (eventObj.Community.Id != request.CommunityId) throw new ApiException("This event doesn't belong to this community");

        eventObj = _mapper.Map<Event>(request);

        await _eventRepository.UpdateAsync(eventObj);
        return new Response<int>(eventObj.Id);
      }
    }
  }
}