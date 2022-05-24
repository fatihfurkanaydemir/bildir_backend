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
namespace Application.Features.Students.Commands.AddFollowedCommunity
{
  public partial class AddFollowedCommunityCommand : IRequest<Response<int>>
  {
    public int CommunityId { get; set; }
    public int StudentId { get; set; }
  }
  public class AddFollowedCommunityCommandHandler : IRequestHandler<AddFollowedCommunityCommand, Response<int>>
  {
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IStudentRepositoryAsync _studentRepository;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IStudentCommunityRepositoryAsync _studentCommunityRepository;

    private readonly IMapper _mapper;

    public AddFollowedCommunityCommandHandler(ICommunityRepositoryAsync communityRepository, IStudentRepositoryAsync studentRepository, IStudentCommunityRepositoryAsync studentCommunityRepository, IAuthenticatedUserService authenticatedUserService, IMapper mapper)
    {
      _communityRepository = communityRepository;
      _studentRepository = studentRepository;
      _authenticatedUserService = authenticatedUserService;
      _studentCommunityRepository = studentCommunityRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(AddFollowedCommunityCommand request, CancellationToken cancellationToken)
    {
      var community = await _communityRepository.GetByIdAsync(request.CommunityId);
      if (community == null) throw new ApiException("Community not found");

      // TODO: Get student by application user id
      var student = await _studentRepository.GetByIdAsync(request.StudentId);
      if (student == null) throw new ApiException("Student not found"); ;

      var studentCommunity = new StudentCommunity
      {
        StudentId = student.Id,
        CommunityId = community.Id,
      };
      
      await _studentCommunityRepository.AddAsync(studentCommunity);

      return new Response<int>(student.Id);
    }
  }
}
