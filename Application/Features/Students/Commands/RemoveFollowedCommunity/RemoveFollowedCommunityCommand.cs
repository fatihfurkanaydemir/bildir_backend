using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Students.Commands.RemoveFollowedCommunity
{
  public partial class RemoveFollowedCommunityCommand : IRequest<Response<int>>
  {
    public int CommunityId { get; set; }
    public int StudentId { get; set; }
  }
  public class RemoveFollowedCommunityCommandHandler : IRequestHandler<RemoveFollowedCommunityCommand, Response<int>>
  {
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IStudentRepositoryAsync _studentRepository;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IStudentCommunityRepositoryAsync _studentCommunityRepository;

    private readonly IMapper _mapper;

    public RemoveFollowedCommunityCommandHandler(ICommunityRepositoryAsync communityRepository, IStudentRepositoryAsync studentRepository, IStudentCommunityRepositoryAsync studentCommunityRepository, IAuthenticatedUserService authenticatedUserService, IMapper mapper)
    {
      _communityRepository = communityRepository;
      _studentRepository = studentRepository;
      _authenticatedUserService = authenticatedUserService;
      _studentCommunityRepository = studentCommunityRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(RemoveFollowedCommunityCommand request, CancellationToken cancellationToken)
    {
      //if (_authenticatedUserService.UserId == null) throw new ApiException("User not logged in");

      var community = await _communityRepository.GetByIdAsync(request.CommunityId);
      if (community == null) throw new ApiException("Community not found");

      var student = await _studentRepository.GetByIdAsync(request.StudentId);
      if (student == null) throw new ApiException("Student not found"); ;

      var studentCommunity = await _studentCommunityRepository.GetStudentCommunityByCompositePKAsync(request.StudentId, request.CommunityId);
      if (studentCommunity == null) throw new ApiException("This student is not following this community");

      await _studentCommunityRepository.DeleteAsync(studentCommunity);

      return new Response<int>(student.Id);
    }
  }
}
