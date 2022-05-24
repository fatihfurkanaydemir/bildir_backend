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


namespace Application.Features.Communities.Queries.GetCommunityByCreationKey
{
  public class GetCommunityByCreationKeyQuery : IRequest<Response<Community>>
  {
    public string CreationKey { get; set; }
    public class GetCommunityByCreationKeyHandler : IRequestHandler<GetCommunityByCreationKeyQuery, Response<Community>>
    {
      private readonly ICommunityRepositoryAsync _communityRepository;
      public GetCommunityByCreationKeyHandler(ICommunityRepositoryAsync communityRepository)
      {
        _communityRepository = communityRepository;
      }
      public async Task<Response<Community>> Handle(GetCommunityByCreationKeyQuery query, CancellationToken cancellationToken)
      {
        var community = await _communityRepository.GetCommunityByCreationKeyAsync(query.CreationKey);
        if (community == null) throw new ApiException($"Community Not Found.");
        return new Response<Community>(community);
      }
    }
  }
}
