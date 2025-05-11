using GradeCom.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GradeCom.Context;

public class GrateContext : IdentityDbContext<User, Role, string>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<GroupSubject> GroupSubjects { get; set; }
    public DbSet<SubjectTeacher> SubjectTeachers { get; set; }
    
    public GrateContext(DbContextOptions<GrateContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GroupSubject>()
            .HasKey(gs => new { gs.GroupId, gs.SubjectId });

        modelBuilder.Entity<SubjectTeacher>()
            .HasKey(st => new { st.SubjectId, st.TeacherId });

        modelBuilder.Entity<SubjectTeacher>()
            .Property(st => st.Role)
            .HasConversion<string>();
    }
}