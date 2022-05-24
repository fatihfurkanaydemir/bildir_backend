using Application.Exceptions;
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


namespace Application.Features.Communities.Commands.UpdateCommunity
{
  public class UpdateCommunityCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string ApplicationUserId { get; set; }
    public string CreationKey { get; set; }
    public bool IsKeyUsed { get; set; }

    public class UpdatePersonnelCommandHandler : IRequestHandler<UpdateCommunityCommand, Response<int>>
    {
      private readonly ICommunityRepositoryAsync _communityRepository;
      private readonly IMapper _mapper;
      public UpdatePersonnelCommandHandler(ICommunityRepositoryAsync communityRepository, IMapper mapper)
      {
        _communityRepository = communityRepository;
        _mapper = mapper;
      }
      public async Task<Response<int>> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
      {
        var community = await _communityRepository.GetByIdAsync(request.Id);

        if (community == null)
        {
          throw new ApiException($"Community Not Found.");
        }
        else
        {
          community = _mapper.Map<Community>(request);
          await _communityRepository.UpdateAsync(community);
          return new Response<int>(community.Id);
        }
      }
    }
  }
}
