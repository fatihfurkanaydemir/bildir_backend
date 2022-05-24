using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Students.Queries.GetAllStudents
{
  public class GetAllStudentsQuery : IRequest<PagedResponse<IEnumerable<GetAllStudentsViewModel>>>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }
  public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, PagedResponse<IEnumerable<GetAllStudentsViewModel>>>
  {
    private readonly IStudentRepositoryAsync _studentRepository;
    private readonly IMapper _mapper;
    public GetAllStudentsQueryHandler(IStudentRepositoryAsync studentRepository, IMapper mapper)
    {
      _studentRepository = studentRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetAllStudentsViewModel>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
      var validFilter = _mapper.Map<GetAllStudentsParameter>(request);
      var students = await _studentRepository.GetStudentsWithRelationsAsync(request.PageNumber, request.PageSize);

      var studentViewModels = new List<GetAllStudentsViewModel>();

      foreach (var s in students)
      {
        var followedCommunities = new List<GetAllStudentsCommunityViewModel>();
        var participatedEvents = new List<GetAllStudentsEventViewModel>();

        foreach (var sc in s.Communities)
          followedCommunities.Add(_mapper.Map<GetAllStudentsCommunityViewModel>(sc.Community));

        foreach (var se in s.Events)
          participatedEvents.Add(_mapper.Map<GetAllStudentsEventViewModel>(se.Event));

        var student = _mapper.Map<GetAllStudentsViewModel>(s);
        student.FollowedCommunities = followedCommunities;
        student.ParticipatedEvents = participatedEvents;
        studentViewModels.Add(student);
      }

      return new PagedResponse<IEnumerable<GetAllStudentsViewModel>>(studentViewModels, validFilter.PageNumber, validFilter.PageSize);
    }
  }
}
