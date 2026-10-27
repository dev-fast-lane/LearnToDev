using LearnToDev.Mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnToDev.Mvc.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{

    //DI Dependency Injection 
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    //Tables of all the modell classes in the ER-Diagram
    public DbSet<Player> Players { get; set; }
    public DbSet<Housing> Housings { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<SchoolLevel> SchoolLevels { get; set; }
    public DbSet<PlayerSchoolProgress> PlayerSchoolProgresses { get; set; }
    public DbSet<StudyQuestion> StudyQuestions { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobRequirement> JobRequirements { get; set; }
    public DbSet<JobPrerequisite> JobPrerequisites { get; set; }
    public DbSet<JobTask> JobTasks { get; set; }
    public DbSet<JobTaskAnswer> JobTaskAnswers { get; set; }
    public DbSet<JobDay> JobDays { get; set; }

    //Rules:
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Combined keys
        modelBuilder.Entity<PlayerSchoolProgress>().HasKey(x => new { x.PlayerId, x.SchoolId });
        modelBuilder.Entity<JobRequirement>().HasKey(x => new { x.JobId, x.SchoolLevelId });
        modelBuilder.Entity<JobPrerequisite>().HasKey(x => new { x.JobId, x.RequiredJobId });

        // JobPrerequisite Has 2 relations to Job
        modelBuilder.Entity<JobPrerequisite>()
        .HasOne(x => x.Job)
        .WithMany(x => x.Prerequisites)
        .HasForeignKey(x => x.JobId)
        .OnDelete(DeleteBehavior.Restrict);

        //JobPrerequisite Has 2 relations to Job
        modelBuilder.Entity<JobPrerequisite>()
        .HasOne(x => x.RequiredJob)
        .WithMany(x => x.RequiredBy)
        .HasForeignKey(x => x.RequiredJobId)
        .OnDelete(DeleteBehavior.Restrict);

        // Player to CurrentJob
        modelBuilder.Entity<Player>()
        .HasOne(x => x.CurrentJob)
        .WithMany(x => x.Players)
        .HasForeignKey(x => x.CurrentJobId)
        .OnDelete(DeleteBehavior.SetNull);
        
        // Player to Housing
        modelBuilder.Entity<Player>()
        .HasOne(x => x.Housing)
        .WithMany(x => x.Players)
        .HasForeignKey(x => x.HousingId)
        .OnDelete(DeleteBehavior.Restrict);


    }
}