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

namespace Application.Features.Students.Queries.GetStudentById
{
  public class GetStudentByIdQuery : IRequest<Response<GetStudentByIdViewModel>>
  {
    public int Id { get; set; }
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, Response<GetStudentByIdViewModel>>
    {
      private readonly IStudentRepositoryAsync _studentRepository;
      private readonly IMapper _mapper;
      public GetStudentByIdQueryHandler(IStudentRepositoryAsync studentRepository, IMapper mapper)
      {
        _studentRepository = studentRepository;
        _mapper = mapper;
      }
      public async Task<Response<GetStudentByIdViewModel>> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken)
      {

        var student = await _studentRepository.GetStudentByIdWithRelationsAsync(query.Id);
        if (student == null) throw new ApiException($"Student Not Found.");

        var followedCommunities = new List<GetStudentByIdCommunityViewModel>();
        var participatedEvents = new List<GetStudentByIdEventViewModel>();

        foreach (var sc in student.Communities)
          followedCommunities.Add(_mapper.Map<GetStudentByIdCommunityViewModel>(sc.Community));

        foreach (var se in student.Events)
          participatedEvents.Add(_mapper.Map<GetStudentByIdEventViewModel>(se.Event));

        var studentViewModel = _mapper.Map<GetStudentByIdViewModel>(student);
        studentViewModel.FollowedCommunities = followedCommunities;
        studentViewModel.ParticipatedEvents = participatedEvents;

        return new Response<GetStudentByIdViewModel>(studentViewModel);
      }
    }
  }
}
