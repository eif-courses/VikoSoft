using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VikoSoft.Entities;

namespace VikoSoft.Data;

public class VikoDbContext : IdentityDbContext
{
    public VikoDbContext(DbContextOptions<VikoDbContext> options) : base(options)
    {
    }

    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<StudyProgram> StudyPrograms { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<PlannedGroup> PlannedGroups { get; set; }

    public DbSet<Plan> Plans { get; set; }
    public DbSet<PlannedSubject> PlannedSubjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConvertUlidsForEntities(modelBuilder);

        LecturerEntityConfiguration.Configure(modelBuilder);
        StudyProgramEntityConfiguration.Configure(modelBuilder);
        SubjectEntityConfiguration.Configure(modelBuilder);
        DepartmentEntityConfiguration.Configure(modelBuilder);
        PlannedGroupEntityConfiguration.Configure(modelBuilder);

        PlanEntityConfiguration.Configure(modelBuilder);
        PlanSubjectEntityConfiguration.Configure(modelBuilder);
        
        SeedData.Initialize(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            
            optionsBuilder.UseSqlite($"Data Source=VikoSoftware.db");
            // var npgsqlConnectionString = new NpgsqlConnectionStringBuilder
            // {
            //     Host = Environment.GetEnvironmentVariable("POSTGRES_HOST"),
            //     Port = Convert.ToInt32(Environment.GetEnvironmentVariable("POSTGRES_PORT")),
            //     Username = Environment.GetEnvironmentVariable("POSTGRES_USER"),
            //     Password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"),
            //     Database = Environment.GetEnvironmentVariable("POSTGRES_DB"),
            //     SslMode = SslMode.Disable, // TODO ENABLE IN PRODUCTION
            // };
            // optionsBuilder.UseNpgsql(npgsqlConnectionString.ConnectionString);
        }
    }


    private void ConvertUlidsForEntities(ModelBuilder modelBuilder)
    {
        var ulidConverter = new UlidValueConverter();

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entity.ClrType.GetProperties()
                .Where(p => p.PropertyType == typeof(Ulid));
            foreach (var property in properties)
            {
                modelBuilder.Entity(entity.ClrType)
                    .Property(property.Name)
                    .HasConversion(ulidConverter);
            }
        }
    }
}