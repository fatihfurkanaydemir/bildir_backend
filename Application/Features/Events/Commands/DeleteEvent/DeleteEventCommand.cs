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

namespace Application.Features.Events.Commands.DeleteEvent
{
  public class DeleteEventCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
  }
  public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, Response<int>>
  {
    private readonly IEventRepositoryAsync _eventRepository;
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IStudentEventRepositoryAsync _studentEventRepository;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IMapper _mapper;
    public DeleteEventCommandHandler(IEventRepositoryAsync eventRepository, ICommunityRepositoryAsync communityRepository, IStudentEventRepositoryAsync studentEventRepository, IAuthenticatedUserService authenticatedUserService, IMapper mapper)
    {
      _eventRepository = eventRepository;
      _communityRepository = communityRepository;
      _studentEventRepository = studentEventRepository;
      _authenticatedUserService = authenticatedUserService;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
      //if (_authenticatedUserService.UserId == null) throw new ApiException("User not logged in");

      var eventObj = await _eventRepository.GetEventByIdWithCommunityAsync(request.Id);
      if (eventObj == null) throw new ApiException("Event not found");

      //if (eventObj.Community.ApplicationUserId != _authenticatedUserService.UserId) throw new ApiException("This event doesn't belong to this community");
      var community = await _communityRepository.GetCommunityByIdWithRelationsAsync(eventObj.Community.Id);
      community.Events.Remove(eventObj);

      var studentEvents = await _studentEventRepository.GetStudentEventsByEventIdAsync(request.Id);
      foreach (var se in studentEvents)
        await _studentEventRepository.DeleteAsync(se);

      await _eventRepository.DeleteAsync(eventObj);
      return new Response<int>(eventObj.Id);
    }
  }
}