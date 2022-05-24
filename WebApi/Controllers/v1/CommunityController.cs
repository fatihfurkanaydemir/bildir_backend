using Application.Features.Communities.Commands.CreateCommunity;
using Application.Features.Communities.Queries.GetAllCommunities;
using Application.Features.Communities.Queries.GetCommunityByCreationKey;
using Application.Features.Communities.Queries.GetCommunityById;
using Application.Features.Communities.Queries.GetLoggedInCommunity;
using Application.Features.Communities.Commands.UpdateCommunity;
using Application.Features.Communities.Commands.AddSocialMediaLinkToCommunity;
using Application.Features.Communities.Commands.AddAvatarImage;
using Application.Features.Communities.Commands.AddBackgroundImage;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApi.Helpers;

namespace WebApi.Controllers.v1
{
  [ApiVersion("1.0")]
  public class CommunityController : BaseApiController
  {
    // GET: api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllCommunitiesParameter filter)
    {
      return Ok(await Mediator.Send(new GetAllCommunitiesQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // GET api/<controller>/key/5
    [HttpGet("key/{creationKey}")]
    public async Task<IActionResult> Get(string creationKey)
    {
      return Ok(await Mediator.Send(new GetCommunityByCreationKeyQuery { CreationKey = creationKey }));
    }

    // GET api/<controller>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      return Ok(await Mediator.Send(new GetCommunityByIdQuery { Id = id }));
    }

    // GET api/<controller>
    [HttpGet("CurrentlyLoggedIn")]
    public async Task<IActionResult> Get()
    {
      return Ok(await Mediator.Send(new GetLoggedInCommunityQuery { }));
    }

    // POST api/<controller>
    [HttpPost]
    //        [Authorize]
    public async Task<IActionResult> Post(CreateCommunityCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // POST api/<controller>
    [HttpPost("AddSocialMediaLinkToCommunity")]
    //        [Authorize]
    public async Task<IActionResult> AddSocialMediaLinkToCommunity(AddSocialMediaLinkToPersonnelCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    //// POST api/<controller>/5
    [HttpPost("AddAvatarImage/{id}")]
    //        [Authorize]
    public async Task<IActionResult> AddAvatarImage(int id, IFormFile file)
    {
      var images = await UploadImagesHelper.UploadImages(Request);
      return Ok(await Mediator.Send(new AddAvatarImageCommand { Image = images[0], Id = id }));
    }

    //// POST api/<controller>/5
    [HttpPost("AddBackgroundImage/{id}")]
    //        [Authorize]
    public async Task<IActionResult> AddBackgroundImage(int id, IFormFile file)
    {
      var image = await UploadImagesHelper.UploadImages(Request);
      return Ok(await Mediator.Send(new AddBackgroundImageCommand { Image = image[0], Id = id }));
    }

    // PUT api/<controller>/5
    [HttpPut("{id}")]
    //[Authorize]
    public async Task<IActionResult> Put(int id, UpdateCommunityCommand command)
    {
      if (id != command.Id)
      {
        return BadRequest();
      }
      return Ok(await Mediator.Send(command));
    }

    // DELETE api/<controller>/5
    //[HttpDelete("{id}")]
    ////       [Authorize]
    //public async Task<IActionResult> Delete(int id)
    //{
    //  return Ok(await Mediator.Send(new DeletePersonnelByIdCommand { Id = id }));
    //}
  }
}
