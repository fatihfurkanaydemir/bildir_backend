using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Queries.GetAllCommunities
{
  public class GetAllCommunitiesQuery : IRequest<PagedResponse<IEnumerable<GetAllCommunitiesViewModel>>>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }
  public class GetAllCommunitiesQueryHandler : IRequestHandler<GetAllCommunitiesQuery, PagedResponse<IEnumerable<GetAllCommunitiesViewModel>>>
  {
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IMapper _mapper;
    public GetAllCommunitiesQueryHandler(ICommunityRepositoryAsync communityRepository, IMapper mapper)
    {
      _communityRepository = communityRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetAllCommunitiesViewModel>>> Handle(GetAllCommunitiesQuery request, CancellationToken cancellationToken)
    {
      var validFilter = _mapper.Map<GetAllCommunitiesParameter>(request);
      var communities = await _communityRepository.GetCommunitiesWithRelationsAsync(request.PageNumber, request.PageSize);

      var communityViewModels = new List<GetAllCommunitiesViewModel>();
      
      foreach (var c in communities)
      {
        var followerStudents = new List<GetAllCommunitiesStudentViewModel>();
        var organizedEvents = new List<GetAllCommunitiesEventViewModel>();

        foreach (var sc in c.Students)
          followerStudents.Add(_mapper.Map<GetAllCommunitiesStudentViewModel>(sc.Student));

        foreach (var e in c.Events)
          organizedEvents.Add(_mapper.Map<GetAllCommunitiesEventViewModel>(e));

        var community = _mapper.Map<GetAllCommunitiesViewModel>(c);
        community.Followers = followerStudents;
        community.OrganizedEvents = organizedEvents;
        communityViewModels.Add(community);
      }

      return new PagedResponse<IEnumerable<GetAllCommunitiesViewModel>>(communityViewModels, validFilter.PageNumber, validFilter.PageSize);
    }
  }
}
