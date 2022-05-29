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

namespace Application.Features.Students.Queries.GetStudentByApplicationUserId
{
  public class GetStudentByApplicationUserIdQuery : IRequest<Response<GetStudentByApplicationUserIdViewModel>>
  {
    public string ApplicationUserId { get; set; }
    public class GetStudentByApplicationUserIdQueryHandler : IRequestHandler<GetStudentByApplicationUserIdQuery, Response<GetStudentByApplicationUserIdViewModel>>
    {
      private readonly IStudentRepositoryAsync _studentRepository;
      private readonly IMapper _mapper;
      public GetStudentByApplicationUserIdQueryHandler(IStudentRepositoryAsync studentRepository, IMapper mapper)
      {
        _studentRepository = studentRepository;
        _mapper = mapper;
      }
      public async Task<Response<GetStudentByApplicationUserIdViewModel>> Handle(GetStudentByApplicationUserIdQuery query, CancellationToken cancellationToken)
      {

        var student = await _studentRepository.GetStudentByApplicationUserIdWithRelationsAsync(query.ApplicationUserId);
        if (student == null) throw new ApiException($"Student Not Found.");

        var followedCommunities = new List<GetStudentByApplicationUserIdCommunityViewModel>();
        var participatedEvents = new List<GetStudentByApplicationUserIdEventViewModel>();

        foreach (var sc in student.Communities)
          followedCommunities.Add(_mapper.Map<GetStudentByApplicationUserIdCommunityViewModel>(sc.Community));

        foreach (var se in student.Events)
        {
          var eventObj = _mapper.Map<GetStudentByApplicationUserIdEventViewModel>(se.Event);
          eventObj.EventOf = _mapper.Map <GetStudentByApplicationUserIdCommunityViewModel> (se.Event.Community);
          eventObj.ParticipationState = se.State.ToString();
          participatedEvents.Add(eventObj);
        }

        var studentViewModel = _mapper.Map<GetStudentByApplicationUserIdViewModel>(student);
        studentViewModel.FollowedCommunities = followedCommunities;
        studentViewModel.ParticipatedEvents = participatedEvents;

        return new Response<GetStudentByApplicationUserIdViewModel>(studentViewModel);
      }
    }
  }
}
