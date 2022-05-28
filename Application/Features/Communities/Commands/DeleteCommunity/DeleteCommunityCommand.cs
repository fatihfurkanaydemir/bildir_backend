using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Commands.DeleteCommunity
{
  public class DeleteCommunityCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
  }
  public class DeleteCommunityCommandHandler : IRequestHandler<DeleteCommunityCommand, Response<int>>
  {
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IMapper _mapper;
    public DeleteCommunityCommandHandler(ICommunityRepositoryAsync communityRepository, IMapper mapper)
    {
      _communityRepository = communityRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(DeleteCommunityCommand request, CancellationToken cancellationToken)
    {
      var community = await _communityRepository.GetByIdAsync(request.Id);
      await _communityRepository.DeleteAsync(community);
      return new Response<int>(community.Id);
    }
  }
}
