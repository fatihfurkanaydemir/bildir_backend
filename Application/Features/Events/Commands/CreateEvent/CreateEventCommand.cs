using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Events.Commands.CreateEvent
{
  public class CreateEventCommand : IRequest<Response<int>>
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string Tags { get; set; }
    public int CommunityId { get; set; }
    public DateTime Date { get; set; }
  }
  public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Response<int>>
  {
    private readonly IEventRepositoryAsync _eventRepository;
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IMapper _mapper;
    public CreateEventCommandHandler(IEventRepositoryAsync eventRepository, ICommunityRepositoryAsync communityRepository, IMapper mapper)
    {
      _eventRepository = eventRepository;
      _communityRepository = communityRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
      var community = await _communityRepository.GetByIdAsync(request.CommunityId);
      if (community == null) throw new ApiException("Community not found");

      var eventObj = _mapper.Map<Event>(request);
      eventObj.State = EventStates.Active;

      await _eventRepository.AddAsync(eventObj);
      return new Response<int>(eventObj.Id);
    }
  }
}
