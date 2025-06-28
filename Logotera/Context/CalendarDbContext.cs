using Logotera.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Logotera.Context;

public class CalendarDbContext : IdentityDbContext<User, Role, string>
{
    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<WeeklyReport> WeeklyReports => Set<WeeklyReport>();

    
    public CalendarDbContext(DbContextOptions<CalendarDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}