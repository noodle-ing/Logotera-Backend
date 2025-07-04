﻿using Microsoft.AspNetCore.Identity;

namespace Logotera.Models;

public class Role : IdentityRole<string>
{
    public string? Description { get; set; }
    public bool CanCreateTest { get; set; }

    public Role() : base() { }
    public Role(string roleName) : base(roleName)
    {
        
    }
    
}