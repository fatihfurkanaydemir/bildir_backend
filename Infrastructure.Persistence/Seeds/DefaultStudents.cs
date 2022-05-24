using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Seeds
{
  public class DefaultStudents
  {
    public static async Task<bool> SeedAsync(IStudentRepositoryAsync studentRepository)
    {
      var student1 = new Student
      {
        FirstName = "Fatih Furkan",
        LastName = "Aydemir",
        SchoolEmail = "20190808043@ogr.akdeniz.edu.tr",
        ApplicationUserId = "85a85290-ebcc-44ad-a3na-1e8f3b458508",
        Department = "Computer Engineering Department",
        Faculty = "Engineering Faculty",
        Gender = "Male",
      };

      var studentList = await studentRepository.GetAllAsync();
      var _product1 = studentList.Where(c => c.FirstName.StartsWith(student1.FirstName)).Count();

      if (_product1 > 0) // ALREADY SEEDED
        return true;


      if (_product1 == 0)
        try
        {
          await studentRepository.AddAsync(student1);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          throw;
        }

      return false; // NOT ALREADY SEEDED

    }
  }
}
