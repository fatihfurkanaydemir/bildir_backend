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

namespace Application.Features.Images.Commands.UploadImage
{
  public class UploadImageCommand : IRequest<Response<ICollection<string>>>
  {
    public ICollection<Image> Images;
  }
  public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, Response<ICollection<string>>>
  {
    private readonly IImageRepositoryAsync _imageRepository;
    private readonly IMapper _mapper;
    public UploadImageCommandHandler(IImageRepositoryAsync imageRepository, IMapper mapper)
    {
      _imageRepository = imageRepository;
      _mapper = mapper;
    }

    public async Task<Response<ICollection<string>>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
      var paths = new List<string>();
      foreach (var image in request.Images)
      {
        await _imageRepository.AddAsync(image);
        paths.Add(image.Path);
      }

      return new Response<ICollection<string>>(paths);
    }
  }
}
