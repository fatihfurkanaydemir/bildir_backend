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

namespace Application.Features.Students.Commands.UpdateStudent
{
  public class UpdateStudentCommand : IRequest<Response<int>>
  {
    public string ApplicationUserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Faculty { get; set; }
    public string Department { get; set; }
    public string Gender { get; set; }

    public class UpdatePersonnelCommandHandler : IRequestHandler<UpdateStudentCommand, Response<int>>
    {
      private readonly IStudentRepositoryAsync _studentRepository;
      private readonly IMapper _mapper;
      public UpdatePersonnelCommandHandler(IStudentRepositoryAsync studentRepository, IMapper mapper)
      {
        _studentRepository = studentRepository;
        _mapper = mapper;
      }
      public async Task<Response<int>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
      {
        /*TODO IMPLEMENT AUTHENTICATED USER*/
        var student = await _studentRepository.GetStudentByApplicationUserIdAsync(request.ApplicationUserId);

        if (student == null)
        {
          throw new ApiException($"Student Not Found.");
        }
        else
        {
          student = _mapper.Map<Student>(request);
          await _studentRepository.UpdateAsync(student);
          return new Response<int>(student.Id);
        }
      }
    }
  }
}
