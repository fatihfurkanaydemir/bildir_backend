using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Queries.GetLoggedInCommunity
{
  public class GetLoggedInCommunityQuery : IRequest<Response<GetLoggedInCommunityViewModel>>
  {
    public class GetLoggedInCommunityQueryHandler : IRequestHandler<GetLoggedInCommunityQuery, Response<GetLoggedInCommunityViewModel>>
    {
      private readonly ICommunityRepositoryAsync _communityRepository;
      private readonly IAuthenticatedUserService _authenticatedUserService;
      private readonly IMapper _mapper;

      public GetLoggedInCommunityQueryHandler(ICommunityRepositoryAsync communityRepository, IAuthenticatedUserService authenticatedUserService, IMapper mapper)
      {
        _communityRepository = communityRepository;
        _authenticatedUserService = authenticatedUserService;
        _mapper = mapper;
      }
      public async Task<Response<GetLoggedInCommunityViewModel>> Handle(GetLoggedInCommunityQuery query, CancellationToken cancellationToken)
      {
        if(_authenticatedUserService.UserId == null) throw new ApiException($"User not logged in.");

        var community = await _communityRepository.GetCommunityByApplicationUserIdWithRelationsAsync(_authenticatedUserService.UserId);
        if (community == null) throw new ApiException($"Community Not Found.");

        var followerStudents = new List<GetLoggedInCommunityStudentViewModel>();
        var organizedEvents = new List<GetLoggedInCommunityEventViewModel>();

        foreach (var sc in community.Students)
          followerStudents.Add(_mapper.Map<GetLoggedInCommunityStudentViewModel>(sc.Student));

        foreach (var e in community.Events)
          organizedEvents.Add(_mapper.Map<GetLoggedInCommunityEventViewModel>(e));

        var communityViewModel = _mapper.Map<GetLoggedInCommunityViewModel>(community);
        communityViewModel.Followers = followerStudents;
        communityViewModel.OrganizedEvents = organizedEvents;

        return new Response<GetLoggedInCommunityViewModel>(communityViewModel);
      }
    }
  }
}
