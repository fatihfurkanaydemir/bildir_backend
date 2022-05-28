using Application.DTOs.Account;
using Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> RegisterCommunityAsync(CommunityRegisterRequest request, string origin, IMediator Mediator);
        Task<Response<string>> RegisterStudentAsync(StudentRegisterRequest request, string origin, IMediator Mediator);
        Task<Response<string>> CreateCommunityAsync(CommunityCreateRequest request, string origin, IMediator Mediator);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Response<string>> DeleteStudentAsync(DeleteUserRequest model, string origin, IMediator Mediator);
        Task<Response<string>> DeleteCommunityAsync(DeleteUserRequest model, string origin, IMediator Mediator);
    }
}
