using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Events.Commands.CreateEvent;
using Application.Features.Events.Commands.CancelEvent;
using Application.Features.Events.Commands.EndEvent;
using Application.Features.Events.Commands.UpdateEvent;
using Application.Features.Events.Commands.AddImagesToEvent;
using Application.Features.Events.Queries.GetAllEvents;
using Application.Features.Events.Queries.GetEventById;
using Application.Features.Events.Queries.GetEventsByStudentId;
using Application.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
using Microsoft.AspNetCore.Http;
using Application.Features.Events.Commands.DeleteEvent;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class EventController : BaseApiController
    {
    // GET: api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllEventsParameter filter)
    {

      return Ok(await Mediator.Send(new GetAllEventsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // GET api/<controller>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      return Ok(await Mediator.Send(new GetEventByIdQuery { Id = id }));
    }

    // GET api/<controller>/GetEventsByStudentId/5
    [HttpGet("GetEventsByStudentId/{studentId}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Get([FromQuery] GetEventsByStudentIdParameter filter, int studentId)
    {
      return Ok(await Mediator.Send(new GetEventsByStudentIdQuery { StudentId = studentId, PageSize = filter.PageSize, PageNumber = filter.PageNumber}));
    }

    //// POST api/<controller>
    [HttpPost]
    [Authorize(Roles = "Community")]
    public async Task<IActionResult> Post(CreateEventCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    //// POST api/<controller>
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(DeleteEventCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    //// POST api/<controller>/5
    [HttpPost("CancelEvent")]
    [Authorize(Roles = "Community")]
    public async Task<IActionResult> CancelEvent(CancelEventCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    //// POST api/<controller>/5
    [HttpPost("EndEvent")]
    [Authorize(Roles = "Community")]
    public async Task<IActionResult> EndEvent(EndEventCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    //// POST api/<controller>/5
    [HttpPost("AddImagesToEvent/{id}")]
    [Authorize(Roles = "Community")]
    public async Task<IActionResult> EndEvent(int id, ICollection<IFormFile> files)
    {
      var images = await UploadImagesHelper.UploadImages(Request);
      return Ok(await Mediator.Send(new AddImagesToEventCommand { Images = images, Id = id}));
    }

    //// POST api/<controller>
    //[HttpPost("AddAddressToEvent")]
    ////        [Authorize]
    //public async Task<IActionResult> AddAddressToEvent(AddAddressToEventCommand command)
    //{
    //    return Ok(await Mediator.Send(command));
    //}

    //// POST api/<controller>
    //[HttpPost("AddParticipantToEvent")]
    ////        [Authorize]
    //public async Task<IActionResult> AddParticipantToEvent(AddParticipantToEventCommand command)
    //{
    //    return Ok(await Mediator.Send(command));
    //}

    // PUT api/<controller>/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Community")]
    public async Task<IActionResult> Put(int id, UpdateEventCommand command)
    {
      if (id != command.Id)
      {
        return BadRequest();
      }
      return Ok(await Mediator.Send(command));
    }

    //// DELETE api/<controller>/5
    //[HttpDelete("{id}")]
    ////       [Authorize]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    return Ok(await Mediator.Send(new DeleteEventByIdCommand { Id = id }));
    //}
  }
}
