using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Students.Commands.CreateStudent
{
  public class CreateStudentCommand : IRequest<Response<int>>
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ApplicationUserId { get; set; }
    public string SchoolEmail { get; set; }
    public string Faculty { get; set; }
    public string Department { get; set; }
    public string Gender { get; set; }
  }
  public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Response<int>>
  {
    private readonly IStudentRepositoryAsync _studentRepository;
    private readonly IMapper _mapper;
    public CreateStudentCommandHandler(IStudentRepositoryAsync studentRepository, IMapper mapper)
    {
      _studentRepository = studentRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
      var student = _mapper.Map<Student>(request);
      await _studentRepository.AddAsync(student);
      return new Response<int>(student.Id);
    }
  }
}
