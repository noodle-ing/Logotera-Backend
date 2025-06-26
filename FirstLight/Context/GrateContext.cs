using FirstLight.Models;
using FirstLight.Models.Files;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstLight.Context;

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
    public DbSet<LectureFile> LectureFiles { get; set; }
    public DbSet<SeminarFile> SeminarFiles { get; set; }
    public DbSet<PracticeFile> PracticeFiles { get; set; }
    public DbSet<HomeTaskFile> HomeTaskFiles { get; set; }
    public DbSet<Module> Modules { get; set; }
    
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
        
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Group)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.SetNull);

        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<LectureFile>()
            .HasOne(f => f.Module)
            .WithMany(t => t.LectureFiles)
            .HasForeignKey(f => f.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SeminarFile>()
            .HasOne(f => f.Module)
            .WithMany(t => t.SeminarFiles)
            .HasForeignKey(f => f.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PracticeFile>()
            .HasOne(f => f.Module)
            .WithMany(t => t.PracticeFiles)
            .HasForeignKey(f => f.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HomeTaskFile>()
            .HasOne(f => f.Module)
            .WithMany(t => t.HomeTaskFiles)
            .HasForeignKey(f => f.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}