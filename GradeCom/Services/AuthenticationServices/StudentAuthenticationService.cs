using GradeCom.Context;
using GradeCom.Dtos.UserDtos;
using GradeCom.Models;

namespace GradeCom.Services.AuthenticationServices;

public class StudentAuthenticationService
{
    private readonly GrateContext _context;

    public StudentAuthenticationService(GrateContext context)
    {
        _context = context;
    }

    public async Task CreateStudentProfileAsync(User user)
    {
        var profile = new Student
        {
            UserId = user.Id,
            User = user
        };

        _context.Students.Add(profile);
        await _context.SaveChangesAsync();
    }
}