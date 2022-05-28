using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Students.Commands.DeleteStudent
{
  public class DeleteStudentCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
  }
  public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, Response<int>>
  {
    private readonly IStudentRepositoryAsync _studentRepository;
    private readonly IMapper _mapper;
    public DeleteStudentCommandHandler(IStudentRepositoryAsync studentRepository, IMapper mapper)
    {
      _studentRepository = studentRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
      var student = await _studentRepository.GetByIdAsync(request.Id);
      if (student == null) throw new ApiException("Student not found");

      await _studentRepository.DeleteAsync(student);
      return new Response<int>(student.Id);
    }
  }
}
