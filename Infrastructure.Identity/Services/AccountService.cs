using Application.DTOs.Account;
using Application.DTOs.Email;
using Application.Enums;
using Application.Exceptions;
using Application.Features.Communities.Commands.CreateCommunity;
using Application.Features.Communities.Queries.GetCommunityByCreationKey;
using Application.Features.Communities.Commands.UpdateCommunity;
using Application.Features.Students.Commands.CreateStudent;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Settings;
using Infrastructure.Identity.Helpers;
using Infrastructure.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Students.Queries.GetStudentByApplicationUserId;
using Application.Features.Students.Commands.DeleteStudent;
using Application.Features.Communities.Queries.GetCommunityByApplicationUserId;
using Application.Features.Communities.Commands.DeleteCommunity;

namespace Infrastructure.Identity.Services
{
  public class AccountService : IAccountService
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailService _emailService;
    private readonly JWTSettings _jwtSettings;
    private readonly IDateTimeService _dateTimeService;

    public AccountService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<JWTSettings> jwtSettings,
        IDateTimeService dateTimeService,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _jwtSettings = jwtSettings.Value;
      _dateTimeService = dateTimeService;
      _signInManager = signInManager;
      this._emailService = emailService;
    }

    public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
    {
      var user = await _userManager.FindByEmailAsync(request.Email);
      if (user == null)
      {
        throw new ApiException($"No Accounts Registered with {request.Email}.");
      }
      var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
      if (!result.Succeeded)
      {
        throw new ApiException($"Invalid Credentials for '{request.Email}'.");
      }
      if (!user.EmailConfirmed)
      {
        throw new ApiException($"Account Not Confirmed for '{request.Email}'.");
      }
      JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
      AuthenticationResponse response = new AuthenticationResponse();
      response.Id = user.Id;
      response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
      response.Email = user.Email;
      response.UserName = user.UserName;
      var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
      response.Roles = rolesList.ToList();
      response.IsVerified = user.EmailConfirmed;
      var refreshToken = GenerateRefreshToken(ipAddress);
      response.RefreshToken = refreshToken.Token;
      return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
    {
      var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
      if (userWithSameUserName != null)
      {
        throw new ApiException($"Username '{request.UserName}' is already taken.");
      }
      var user = new ApplicationUser
      {
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName,
        UserName = request.UserName
      };
      var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
      if (userWithSameEmail == null)
      {
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
          await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
          var verificationUri = await SendVerificationEmail(user, origin);
          //Attach Email Service here and configure it via appsettings
          //await _emailService.SendAsync(new Application.DTOs.Email.EmailRequest() { From = "mail@codewithmukesh.com", To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
          return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
        }
        else
        {
          throw new ApiException($"{result.Errors}");
        }
      }
      else
      {
        throw new ApiException($"Email {request.Email } is already registered.");
      }
    }

    public async Task<Response<int>> RegisterCommunityAsync(CommunityRegisterRequest request, string origin, IMediator Mediator)
    {
      /*
       Check if the CreationKey is valid
       */
      var getCommunityResult = await Mediator.Send(new GetCommunityByCreationKeyQuery { CreationKey = request.CreationKey });
      if (!getCommunityResult.Succeeded) throw new ApiException($"Creation Key not found.");

      var community = getCommunityResult.Data;
      if(community.IsKeyUsed) throw new ApiException($"Creation Key already used.");

      var user = new ApplicationUser
      {
        Email = request.Email,
        FirstName = request.Name,
        UserName = request.Name.ToLower().Trim().Replace(" ", ""),
        EmailConfirmed = true
      };
      var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
      if (userWithSameEmail == null)
      {
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
          await _userManager.AddToRoleAsync(user, Roles.Community.ToString());
          //var verificationUri = await SendVerificationEmail(user, origin);
          // Attach Email Service here and configure it via appsettings
          //await _emailService.SendAsync(new Application.DTOs.Email.EmailRequest() { From = "mail@codewithmukesh.com", To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });

          // Mediator update community
          var updateCommunityResult = await Mediator.Send(new UpdateCommunityCommand
          {
            ApplicationUserId = user.Id,
            Id = community.Id,
            CreationKey = community.CreationKey,
            Description = request.Description,
            Email = request.Email,
            IsKeyUsed = true,
            Name = request.Name,
          });

          if (!updateCommunityResult.Succeeded) throw new ApiException($"Community could not be updated");
          // return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
          return new Response<int>(community.Id, message: $"User Registered.");
        }
        else
        {
          throw new ApiException($"{result.Errors}");
        }
      }
      else
      {
        throw new ApiException($"Email {request.Email } is already registered.");
      }
    }

    public async Task<Response<string>> RegisterStudentAsync(StudentRegisterRequest request, string origin, IMediator Mediator)
    {

      /*Check if mail is school mail*/
      //if(!request.SchoolEmail.EndsWith("akdeniz.edu.tr")) throw new ApiException($"Email adress is not valid");

      
      var user = new ApplicationUser
      {
        Email = request.SchoolEmail,
        UserName = request.SchoolEmail.ToLower().Trim().Replace(".", "").Replace("@", "").Replace("+", ""),
        EmailConfirmed = true
      };
      var userWithSameEmail = await _userManager.FindByEmailAsync(request.SchoolEmail);
      if (userWithSameEmail == null)
      {
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
          await _userManager.AddToRoleAsync(user, Roles.Student.ToString());
          //var verificationUri = await SendVerificationEmail(user, origin);
          // Attach Email Service here and configure it via appsettings
          //await _emailService.SendAsync(new Application.DTOs.Email.EmailRequest() { From = "mail@codewithmukesh.com", To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });

          // Mediator update community
          var createStudentResult = await Mediator.Send(new CreateStudentCommand
          {
            ApplicationUserId = user.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Department = request.Department,
            SchoolEmail = request.SchoolEmail,
            Faculty = request.Faculty,
            Gender = request.Gender
          });

          if (!createStudentResult.Succeeded) throw new ApiException($"Student could not be created");
          // return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
          return new Response<string>(user.Id, message: $"User Registered.");
        }
        else
        {
          throw new ApiException($"{result.Errors}");
        }
      }
      else
      {
        throw new ApiException($"Email {request.SchoolEmail } is already registered.");
      }
    }

    public async Task<Response<string>> CreateCommunityAsync(CommunityCreateRequest request, string origin, IMediator Mediator)
    {
      var command = new CreateCommunityCommand
      {
        Name = request.Name,
        Description = "Unknown",
        ApplicationUserId = "None",
        Email = "Unknown",
        IsKeyUsed = false,
        CreationKey = Guid.NewGuid().ToString()
      };

      var result = await Mediator.Send(command);

      if (!result.Succeeded) throw new ApiException($"Could not create community");
      return new Response<string>(command.CreationKey, message: $"Community ({command.Name}) has been created. Key: {command.CreationKey}");
    }

    private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
    {
      var userClaims = await _userManager.GetClaimsAsync(user);
      var roles = await _userManager.GetRolesAsync(user);

      var roleClaims = new List<Claim>();

      for (int i = 0; i < roles.Count; i++)
      {
        roleClaims.Add(new Claim("roles", roles[i]));
      }

      string ipAddress = IpHelper.GetIpAddress();

      var claims = new[]
      {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
      .Union(userClaims)
      .Union(roleClaims);

      var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
      var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

      var jwtSecurityToken = new JwtSecurityToken(
          issuer: _jwtSettings.Issuer,
          audience: _jwtSettings.Audience,
          claims: claims,
          expires: DateTime.UtcNow.AddDays(_jwtSettings.DurationInDays),
          signingCredentials: signingCredentials);
      return jwtSecurityToken;
    }

    private string RandomTokenString()
    {
      using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
      var randomBytes = new byte[40];
      rngCryptoServiceProvider.GetBytes(randomBytes);
      // convert random bytes to hex string
      return BitConverter.ToString(randomBytes).Replace("-", "");
    }

    private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
    {
      var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
      var route = "api/account/confirm-email/";
      var _enpointUri = new Uri(string.Concat($"{origin}/", route));
      var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
      verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
      //Email Service Call Here
      return verificationUri;
    }

    public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
    {
      var user = await _userManager.FindByIdAsync(userId);
      code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
      var result = await _userManager.ConfirmEmailAsync(user, code);
      if (result.Succeeded)
      {
        return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
      }
      else
      {
        throw new ApiException($"An error occured while confirming {user.Email}.");
      }
    }

    private RefreshToken GenerateRefreshToken(string ipAddress)
    {
      return new RefreshToken
      {
        Token = RandomTokenString(),
        Expires = DateTime.UtcNow.AddDays(7),
        Created = DateTime.UtcNow,
        CreatedByIp = ipAddress
      };
    }

    public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
    {
      var account = await _userManager.FindByEmailAsync(model.Email);

      // always return ok response to prevent email enumeration
      if (account == null) return;

      var code = await _userManager.GeneratePasswordResetTokenAsync(account);
      var route = "api/account/reset-password/";
      var _enpointUri = new Uri(string.Concat($"{origin}/", route));
      var emailRequest = new EmailRequest()
      {
        Body = $"You reset token is - {code}",
        To = model.Email,
        Subject = "Reset Password",
      };
      //await _emailService.SendAsync(emailRequest);
    }

    public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
    {
      var account = await _userManager.FindByEmailAsync(model.Email);
      if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
      var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
      if (result.Succeeded)
      {
        return new Response<string>(model.Email, message: $"Password Resetted.");
      }
      else
      {
        throw new ApiException($"Error occured while reseting the password.");
      }
    }

    public async Task<Response<string>> DeleteStudentAsync(DeleteUserRequest model, string origin, IMediator Mediator)
    {
      var account = await _userManager.FindByEmailAsync(model.Email);
      if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");

      var student = await Mediator.Send(new GetStudentByApplicationUserIdQuery { ApplicationUserId = account.Id });
      if(student == null) throw new ApiException($"No students related with {model.Email}.");
      
      await Mediator.Send(new DeleteStudentCommand { Id = student.Data.Id });
      var result = await _userManager.DeleteAsync(account);
      if (result.Succeeded)
      {
        return new Response<string>(model.Email, message: $"User deleted.");
      }
      else
      {
        throw new ApiException($"Error occured while deleting the user.");
      }
    }

    public async Task<Response<string>> DeleteCommunityAsync(DeleteUserRequest model, string origin, IMediator Mediator)
    {
      var account = await _userManager.FindByEmailAsync(model.Email);
      if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");

      var community = await Mediator.Send(new GetCommunityByApplicationUserIdQuery { ApplicationUserId = account.Id });
      if (community == null) throw new ApiException($"No students related with {model.Email}.");

      await Mediator.Send(new DeleteCommunityCommand { Id = community.Data.Id });
      var result = await _userManager.DeleteAsync(account);
      if (result.Succeeded)
      {
        return new Response<string>(model.Email, message: $"User deleted.");
      }
      else
      {
        throw new ApiException($"Error occured while deleting the user.");
      }
    }
  }

}
