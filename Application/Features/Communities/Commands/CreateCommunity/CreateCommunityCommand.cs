using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Commands.CreateCommunity
{
  public class CreateCommunityCommand : IRequest<Response<int>>
  {
    public string CreationKey { get; set; }
    public bool IsKeyUsed { get; set; }
    public string ApplicationUserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
  }
  public class CreateCommunityCommandHandler : IRequestHandler<CreateCommunityCommand, Response<int>>
  {
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IMapper _mapper;
    public CreateCommunityCommandHandler(ICommunityRepositoryAsync communityRepository, IMapper mapper)
    {
      _communityRepository = communityRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
    {
      var community = _mapper.Map<Community>(request);
      await _communityRepository.AddAsync(community);
      return new Response<int>(community.Id);
    }
  }
}
