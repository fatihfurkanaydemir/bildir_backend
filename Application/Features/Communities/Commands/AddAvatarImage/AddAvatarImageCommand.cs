using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Communities.Commands.AddAvatarImage
{
  public class AddAvatarImageCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
    public Image Image;
  }
  public class AddAvatarImageCommandHandler : IRequestHandler<AddAvatarImageCommand, Response<int>>
  {
    private readonly IImageRepositoryAsync _imageRepository;
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IMapper _mapper;
    public AddAvatarImageCommandHandler(IImageRepositoryAsync imageRepository, ICommunityRepositoryAsync communityRepository, IAuthenticatedUserService authenticatedUserService, IMapper mapper)
    {
      _imageRepository = imageRepository;
      _communityRepository = communityRepository;
      _authenticatedUserService = authenticatedUserService;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(AddAvatarImageCommand request, CancellationToken cancellationToken)
    {
      //if (_authenticatedUserService.UserId == null) throw new ApiException("User not logged in");

      var community = await _communityRepository.GetByIdAsync(request.Id);
      if (community == null) throw new ApiException("Community not found");

      await _imageRepository.AddAsync(request.Image);

      community.Avatar = request.Image;
      await _communityRepository.UpdateAsync(community);

      return new Response<int>(community.Id);
    }
  }
}
