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

namespace Application.Features.Notifications.Queries.GetAllNotifications
{
  public class GetAllNotificationsQuery : IRequest<PagedResponse<IEnumerable<GetAllNotificationsViewModel>>>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }
  public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, PagedResponse<IEnumerable<GetAllNotificationsViewModel>>>
  {
    private readonly INotificationRepositoryAsync _notificationRepository;
    private readonly IMapper _mapper;
    public GetAllNotificationsQueryHandler( INotificationRepositoryAsync notificationRepository,  IMapper mapper)
    {
      _notificationRepository = notificationRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetAllNotificationsViewModel>>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
      var validFilter = _mapper.Map<GetAllNotificationsParameter>(request);
      var notifications = await _notificationRepository.GetNotificationsWithRelationsAsync(request.PageNumber, request.PageSize);
      
      var notificationViewModels = new List<GetAllNotificationsViewModel>();
      foreach (var n in notifications)
      {
        var community = _mapper.Map<GetAllNotificationsCommunityViewModel>(n.Community);
        var notification = _mapper.Map<GetAllNotificationsViewModel>(n);
        notification.SentBy = community;

        notificationViewModels.Add(notification);
      }

      return new PagedResponse<IEnumerable<GetAllNotificationsViewModel>>(notificationViewModels, validFilter.PageNumber, validFilter.PageSize);
    }
  }
}
