using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace Application.Features.Students.Queries.GetLoggedInStudent
{
  public class GetLoggedInStudentQuery : IRequest<Response<GetLoggedInStudentViewModel>>
  {
    public class GetLoggedInStudentQueryHandler : IRequestHandler<GetLoggedInStudentQuery, Response<GetLoggedInStudentViewModel>>
    {
      private readonly IStudentRepositoryAsync _studentRepository;
      private readonly IAuthenticatedUserService _authenticatedUserService;
      private readonly IMapper _mapper;

      public GetLoggedInStudentQueryHandler(IStudentRepositoryAsync studentRepository, IAuthenticatedUserService authenticatedUserService, IMapper mapper)
      {
        _studentRepository = studentRepository;
        _authenticatedUserService = authenticatedUserService;
        _mapper = mapper;
      }
      public async Task<Response<GetLoggedInStudentViewModel>> Handle(GetLoggedInStudentQuery query, CancellationToken cancellationToken)
      {
        if (_authenticatedUserService.UserId == null) throw new ApiException($"User not logged in.");

        var student = await _studentRepository.GetStudentByApplicationUserIdWithRelationsAsync(_authenticatedUserService.UserId);
        if (student == null) throw new ApiException($"Student Not Found.");

        var followedCommunities = new List<GetLoggedInStudentCommunityViewModel>();
        var participatedEvents = new List<GetLoggedInStudentEventViewModel>();

        foreach (var sc in student.Communities) 
          followedCommunities.Add(_mapper.Map<GetLoggedInStudentCommunityViewModel>(sc.Community));

        foreach (var se in student.Events)
          participatedEvents.Add(_mapper.Map<GetLoggedInStudentEventViewModel>(se.Event));

        var studentViewModel = _mapper.Map<GetLoggedInStudentViewModel>(student);
        studentViewModel.FollowedCommunities = followedCommunities;
        studentViewModel.ParticipatedEvents = participatedEvents;

        return new Response<GetLoggedInStudentViewModel>(studentViewModel);
      }
    }
  }
}