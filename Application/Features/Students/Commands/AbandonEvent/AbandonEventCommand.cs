using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Students.Commands.AbandonEvent
{
  public class AbandonEventCommand : IRequest<Response<int>>
  {
    public int EventId { get; set; }
    public int StudentId { get; set; }
    
  }
  public class AbandonEventCommandHandler : IRequestHandler<AbandonEventCommand, Response<int>>
  {
    private readonly IStudentRepositoryAsync _studentRepository;
    private readonly IEventRepositoryAsync _eventRepository;
    private readonly IStudentEventRepositoryAsync _studentEventRepository;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    public AbandonEventCommandHandler(IStudentRepositoryAsync studentRepository, IEventRepositoryAsync eventRepository, IStudentEventRepositoryAsync studentEventRepository, IAuthenticatedUserService authenticatedUserService)
    {
      _studentRepository = studentRepository;
      _eventRepository = eventRepository;
      _studentEventRepository = studentEventRepository;
      _authenticatedUserService = authenticatedUserService;
    }

    public async Task<Response<int>> Handle(AbandonEventCommand request, CancellationToken cancellationToken)
    {
      /*TODO*/
      //if(_authenticatedUserService.UserId == null) throw new ApiException("User not logged in");

      var student = await _studentRepository.GetByIdAsync(request.StudentId);
      if (student == null) throw new ApiException("Student not found");

      var eventObj = await _eventRepository.GetByIdAsync(request.EventId);
      if (eventObj == null) throw new ApiException("Event not found");
      if (eventObj.State == EventStates.Ended) throw new ApiException("This event is ended");
      if (eventObj.State == EventStates.Canceled) throw new ApiException("This event is canceled");

      var studentEvent = await _studentEventRepository.GetStudentEventByCompositePKAsync(student.Id, eventObj.Id);
      if(studentEvent == null) throw new ApiException("Student never registered to this event");
      
      if(studentEvent.State == StudentEventStates.Abandoned) throw new ApiException("Student already abandoned this event");
      if(studentEvent.State == StudentEventStates.Participated) throw new ApiException("Student already participated this event");

      studentEvent.State = StudentEventStates.Abandoned;
      await _studentEventRepository.UpdateAsync(studentEvent);

      return new Response<int>(student.Id);
    }
  }
}
