using Microsoft.AspNetCore.Identity;

namespace GradeCom.Models;

public class User : IdentityUser
{
    public string Surname { get; set; }
}