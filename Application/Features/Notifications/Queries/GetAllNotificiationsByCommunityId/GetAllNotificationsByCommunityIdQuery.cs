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

namespace Application.Features.Notifications.Queries.GetAllNotificationsByCommunityId
{
  public class GetAllNotificationsByCommunityIdQuery : IRequest<PagedResponse<IEnumerable<GetAllNotificationsByCommunityIdViewModel>>>
  {
    public int CommunityId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }
  public class GetAllNotificationsByCommunityIdQueryHandler : IRequestHandler<GetAllNotificationsByCommunityIdQuery, PagedResponse<IEnumerable<GetAllNotificationsByCommunityIdViewModel>>>
  {
    private readonly INotificationRepositoryAsync _notificationRepository;
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IMapper _mapper;
    public GetAllNotificationsByCommunityIdQueryHandler( INotificationRepositoryAsync notificationRepository, ICommunityRepositoryAsync communityRepository,  IMapper mapper)
    {
      _notificationRepository = notificationRepository;
      _communityRepository = communityRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetAllNotificationsByCommunityIdViewModel>>> Handle(GetAllNotificationsByCommunityIdQuery request, CancellationToken cancellationToken)
    {
      var community = await _communityRepository.GetByIdAsync(request.CommunityId);
      if (community == null) throw new ApiException("Community not found");

      var validFilter = _mapper.Map<GetAllNotificationsByCommunityIdParameter>(request);
      var notifications = await _notificationRepository.GetNotificationsByCommunityIdAsync(request.CommunityId, request.PageNumber, request.PageSize);
      
      var notificationViewModels = _mapper.Map<IEnumerable<GetAllNotificationsByCommunityIdViewModel>>(notifications);
      return new PagedResponse<IEnumerable<GetAllNotificationsByCommunityIdViewModel>>(notificationViewModels, validFilter.PageNumber, validFilter.PageSize);
    }
  }
}
