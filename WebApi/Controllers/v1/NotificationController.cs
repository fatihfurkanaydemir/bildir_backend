using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Notifications.Commands.CreateNotification;
using Application.Features.Notifications.Queries.GetAllNotifications;
using Application.Features.Notifications.Queries.GetAllNotificationsByCommunityId;
using Application.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class NotificationController : BaseApiController
    {
    // GET: api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllNotificationsParameter filter)
    {

      return Ok(await Mediator.Send(new GetAllNotificationsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // GET api/<controller>/5
    [HttpGet("{communityId}")]
    public async Task<IActionResult> Get([FromQuery] GetAllNotificationsByCommunityIdParameter filter, int communityId)
    {
      return Ok(await Mediator.Send(new GetAllNotificationsByCommunityIdQuery { CommunityId = communityId, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    //// POST api/<controller>
    [HttpPost]
    [Authorize(Roles = "Community")]
    public async Task<IActionResult> Post(CreateNotificationCommand command)
    {
      return Ok(await Mediator.Send(command));
    }
  }
}
