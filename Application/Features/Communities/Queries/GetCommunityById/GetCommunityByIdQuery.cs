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
using Application.Features.Communities.Queries.GetAllCommunities;
using AutoMapper;

namespace Application.Features.Communities.Queries.GetCommunityById
{
  public class GetCommunityByIdQuery : IRequest<Response<GetCommunityByIdViewModel>>
  {
    public int Id { get; set; }
    public class GetCommunityByIdQueryHandler : IRequestHandler<GetCommunityByIdQuery, Response<GetCommunityByIdViewModel>>
    {
      private readonly ICommunityRepositoryAsync _communityRepository;
      private readonly IMapper _mapper;
      public GetCommunityByIdQueryHandler(ICommunityRepositoryAsync communityRepository, IMapper mapper)
      {
        _communityRepository = communityRepository;
        _mapper = mapper;
      }
      public async Task<Response<GetCommunityByIdViewModel>> Handle(GetCommunityByIdQuery query, CancellationToken cancellationToken)
      {
        var community = await _communityRepository.GetCommunityByIdWithRelationsAsync(query.Id);
        if (community == null) throw new ApiException($"Community Not Found.");

        var followerStudents = new List<GetCommunityByIdStudentViewModel>();
        var organizedEvents = new List<GetCommunityByIdEventViewModel>();

        foreach (var sc in community.Students)
          followerStudents.Add(_mapper.Map<GetCommunityByIdStudentViewModel>(sc.Student));

        foreach (var  e in community.Events)
          organizedEvents.Add(_mapper.Map<GetCommunityByIdEventViewModel>(e));

        var communityViewModel = _mapper.Map<GetCommunityByIdViewModel>(community);
        communityViewModel.Followers = followerStudents;
        communityViewModel.OrganizedEvents = organizedEvents;

        return new Response<GetCommunityByIdViewModel>(communityViewModel);
      }
    }
  }
}
