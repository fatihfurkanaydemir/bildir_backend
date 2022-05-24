using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Reports.Commands.CreateReport;
using Application.Features.Reports.Commands.SolveReport;
using Application.Features.Reports.Queries.GetAllReports;
using Application.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ReportController : BaseApiController
    {
    // GET: api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllReportsParameter filter)
    {

      return Ok(await Mediator.Send(new GetAllReportsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    //// POST api/<controller>
    [HttpPost]
    //        [Authorize]
    public async Task<IActionResult> Post(CreateReportCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    //// POST api/<controller>
    [HttpPost("SolveReport")]
    //        [Authorize]
    public async Task<IActionResult> Post(SolveReportCommand command)
    {
      return Ok(await Mediator.Send(command));
    }
  }
}
