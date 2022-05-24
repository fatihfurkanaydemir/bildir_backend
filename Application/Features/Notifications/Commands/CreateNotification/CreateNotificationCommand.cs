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

namespace Application.Features.Notifications.Commands.CreateNotification
{
  public class CreateNotificationCommand : IRequest<Response<int>>
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public int CommunityId { get; set; }
  }
  public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Response<int>>
  {
    private readonly INotificationRepositoryAsync _notificationRepository;
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IMapper _mapper;
    public CreateNotificationCommandHandler(INotificationRepositoryAsync notificationRepository, ICommunityRepositoryAsync communityRepository, IMapper mapper)
    {
      _notificationRepository = notificationRepository;
      _communityRepository = communityRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
      /*
       TODO IMPLEMENT AUTHENTICATED
       */
      var community = await _communityRepository.GetCommunityByIdWithRelationsAsync(request.CommunityId);
      if (community == null) throw new ApiException("Community not found");

      var notification = _mapper.Map<Notification>(request);
      notification.CommunityId = request.CommunityId;

      /*/////////////////////////////
       SEND NOTIFICATIONS TO STUDENTS
       ////////////////////////////*/

      await _notificationRepository.AddAsync(notification);
      return new Response<int>(notification.Id);
    }
  }
}
