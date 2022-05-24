using Application.Exceptions;
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

namespace Application.Features.Communities.Commands.AddBackgroundImage
{
  public class AddBackgroundImageCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
    public Image Image;
  }
  public class AddBackgroundImageCommandHandler : IRequestHandler<AddBackgroundImageCommand, Response<int>>
  {
    private readonly IImageRepositoryAsync _imageRepository;
    private readonly ICommunityRepositoryAsync _communityRepository;
    private readonly IMapper _mapper;
    public AddBackgroundImageCommandHandler(IImageRepositoryAsync imageRepository, ICommunityRepositoryAsync communityRepository, IMapper mapper)
    {
      _imageRepository = imageRepository;
      _communityRepository = communityRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(AddBackgroundImageCommand request, CancellationToken cancellationToken)
    {
      /*
       TODO AUTHENTICATED USER SERVICE
       */
      var community = await _communityRepository.GetByIdAsync(request.Id);
      if (community == null) throw new ApiException("Community not found");

      await _imageRepository.AddAsync(request.Image);

      community.BackgroundImage = request.Image;
      await _communityRepository.UpdateAsync(community);

      return new Response<int>(community.Id);
    }
  }
}
