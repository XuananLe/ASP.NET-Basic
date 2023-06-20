using BookService.Models;
using System.Linq;
namespace BookService.Data;

public class StudentRepository
{
    private readonly SchoolContext _context;

    public StudentRepository(SchoolContext context)
    {
        _context = context;
    }

    public List<Student> GetAllStudents()
    {
        return _context.Students.ToList(); 
    }
}