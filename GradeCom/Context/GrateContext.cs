using GradeCom.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GradeCom.Context;

public class GrateContext : IdentityDbContext<User>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Grade> Grades { get; set; }
    
    public GrateContext(DbContextOptions<GrateContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}