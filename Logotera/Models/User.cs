using Microsoft.AspNetCore.Identity;

namespace Logotera.Models;

public class User : IdentityUser
{
    public string Surname { get; set; }
    public string? Description { get; set; }
    public string? ProfileImagePath { get; set; } 
}