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

namespace Application.Features.Events.Commands.AddImagesToEvent
{
  public class AddImagesToEventCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
    public ICollection<Image> Images;
  }
  public class AddImagesToEventCommandHandler : IRequestHandler<AddImagesToEventCommand, Response<int>>
  {
    private readonly IImageRepositoryAsync _imageRepository;
    private readonly IEventRepositoryAsync _eventRepository;
    private readonly IMapper _mapper;
    public AddImagesToEventCommandHandler(IImageRepositoryAsync imageRepository, IEventRepositoryAsync eventRepository, IMapper mapper)
    {
      _imageRepository = imageRepository;
      _eventRepository = eventRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(AddImagesToEventCommand request, CancellationToken cancellationToken)
    {
      var eventObj = await _eventRepository.GetByIdAsync(request.Id);
      if (eventObj == null) throw new ApiException("Event not found");

      var paths = new List<string>();
      foreach (var image in request.Images)
        await _imageRepository.AddAsync(image);

      eventObj.Images = request.Images;
      await _eventRepository.UpdateAsync(eventObj);
      

      return new Response<int>(request.Images.Count);
    }
  }
}
