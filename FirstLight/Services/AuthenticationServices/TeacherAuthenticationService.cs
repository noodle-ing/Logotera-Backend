using FirstLight.Context;
using FirstLight.Models;

namespace FirstLight.Services.AuthenticationServices;

public class TeacherAuthenticationService
{
    private readonly GrateContext _context;

    public TeacherAuthenticationService(GrateContext context)
    {
        _context = context;
    }

    public async Task CreateTeacherProfileAsync(User user)
    {
        var profile = new Teacher
        {
            UserId = user.Id,
            User = user
        };

        _context.Teachers.Add(profile);
        await _context.SaveChangesAsync();
    }


}