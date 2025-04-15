using Microsoft.AspNetCore.Identity;

namespace GradeCom.Models;

public class Role : IdentityRole
{
    public string? Description { get; set; }
    public bool CanCreateTest { get; set; }

    public Role() { }

    public Role(string roleName) : base(roleName)
    {
        
    }
    
}