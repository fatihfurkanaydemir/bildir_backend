using Application.Exceptions;
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

namespace Application.Features.Communities.Queries.GetCommunityByApplicationUserId
{
  public class GetCommunityByApplicationUserIdQuery : IRequest<Response<GetCommunityByApplicationUserIdViewModel>>
  {
    public string ApplicationUserId { get; set; }
    public class GetCommunityByApplicationUserIdQueryHandler : IRequestHandler<GetCommunityByApplicationUserIdQuery, Response<GetCommunityByApplicationUserIdViewModel>>
    {
      private readonly ICommunityRepositoryAsync _communityRepository;
      private readonly IMapper _mapper;
      public GetCommunityByApplicationUserIdQueryHandler(ICommunityRepositoryAsync communityRepository, IMapper mapper)
      {
        _communityRepository = communityRepository;
        _mapper = mapper;
      }
      public async Task<Response<GetCommunityByApplicationUserIdViewModel>> Handle(GetCommunityByApplicationUserIdQuery query, CancellationToken cancellationToken)
      {
        var community = await _communityRepository.GetCommunityByApplicationUserIdWithRelationsAsync(query.ApplicationUserId);
        if (community == null) throw new ApiException($"Community Not Found.");

        var followerStudents = new List<GetCommunityByApplicationUserIdStudentViewModel>();
        var organizedEvents = new List<GetCommunityByApplicationUserIdEventViewModel>();

        foreach (var sc in community.Students)
          followerStudents.Add(_mapper.Map<GetCommunityByApplicationUserIdStudentViewModel>(sc.Student));

        foreach (var  e in community.Events)
          organizedEvents.Add(_mapper.Map<GetCommunityByApplicationUserIdEventViewModel>(e));

        var communityViewModel = _mapper.Map<GetCommunityByApplicationUserIdViewModel>(community);
        communityViewModel.Followers = followerStudents;
        communityViewModel.OrganizedEvents = organizedEvents;

        return new Response<GetCommunityByApplicationUserIdViewModel>(communityViewModel);
      }
    }
  }
}
