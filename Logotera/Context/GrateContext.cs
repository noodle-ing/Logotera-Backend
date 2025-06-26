using Logotera.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Logotera.Context;

public class GrateContext : IdentityDbContext<User, Role, string>
{
    public DbSet<User> Users { get; set; }

    
    public GrateContext(DbContextOptions<GrateContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}