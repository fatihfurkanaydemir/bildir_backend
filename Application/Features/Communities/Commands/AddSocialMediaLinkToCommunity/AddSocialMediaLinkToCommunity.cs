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

namespace Application.Features.Communities.Commands.AddSocialMediaLinkToCommunity
{
  public partial class AddSocialMediaLinkToPersonnelCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
    public string? InstagramLink { get; set; }
    public string? TwitterLink { get; set; }
    public string? FacebookLink { get; set; }
    public string? LinkedinLink { get; set; }

  }
  public class AddSocialMediaLinkToPersonnelCommandHandler : IRequestHandler<AddSocialMediaLinkToPersonnelCommand, Response<int>>
  {
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IAuthenticatedUserService _authenticatedUserService;

    private readonly IMapper _mapper;

    public AddSocialMediaLinkToPersonnelCommandHandler(ICommunityRepositoryAsync communityRepository, IAuthenticatedUserService authenticatedUserService, IMapper mapper)
    {
      _communityRepository = communityRepository;
      _authenticatedUserService = authenticatedUserService;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(AddSocialMediaLinkToPersonnelCommand request, CancellationToken cancellationToken)
    {
      if (_authenticatedUserService.UserId == null) throw new ApiException("User not logged in");

      var community = await _communityRepository.GetByIdAsync(request.Id);
      if (community == null) return null;

      community.InstagramLink = request.InstagramLink;
      community.LinkedinLink = request.LinkedinLink;
      community.TwitterLink = request.TwitterLink;
      community.FacebookLink = request.FacebookLink;

      await _communityRepository.UpdateAsync(community);

      return new Response<int>(community.Id);
    }
  }
}
