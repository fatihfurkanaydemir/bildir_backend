using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Account;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Communities.Commands.CreateCommunity;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAddress()));
        }
        //[HttpPost("register")]
        //public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        //{
        //    var origin = Request.Headers["origin"];
        //    return Ok(await _accountService.RegisterAsync(request, origin));
        //}
        [HttpPost("register-community")]
        public async Task<IActionResult> RegisterCommunityAsync(CommunityRegisterRequest request)
        {
          var origin = Request.Headers["origin"];
          return Ok(await _accountService.RegisterCommunityAsync(request, origin, Mediator));
        }
        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudentAsync(StudentRegisterRequest request)
        {
          var origin = Request.Headers["origin"];
          return Ok(await _accountService.RegisterStudentAsync(request, origin, Mediator));
        }
        [HttpPost("create-community")]
        public async Task<IActionResult> CreateCommunityAsync(CommunityCreateRequest request)
        {
          var origin = Request.Headers["origin"];
          return Ok(await _accountService.CreateCommunityAsync(request, origin, Mediator));
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery]string userId, [FromQuery]string code)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            
            return Ok(await _accountService.ResetPassword(model));
        }
        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress==null ? "127.0.0.1":HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}