using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tarifikacija.Entities;
using VikoSoft.Data.Entities;

namespace VikoSoft.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    // Magic string.
    public static readonly string RowVersion = nameof(RowVersion);

    // Magic strings.
    public static readonly string ApplicationDb = nameof(ApplicationDb).ToLower();


    // public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    // {
    //     Debug.WriteLine($"{ContextId} context created.");
    // }

    public DbSet<Category> Categories { get; set; }
    public DbSet<SubjectType> SubjectTypes { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<StudyPlan> StudyPlans { get; set; }
    public DbSet<StudyProgram> StudyPrograms { get; set; }
    public DbSet<StudyForm> StudyForms { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<ContactHours> ContactHours { get; set; }
    public DbSet<NonContactHours> NonContactHours { get; set; }
    public DbSet<ContactHoursDetails> ContactHoursDetails { get; set; }
    public DbSet<NonContactHoursDetails> NonContactHoursDetails { get; set; }
    
    public DbSet<TeacherCard> TeacherCards { get; set; }
    public DbSet<TeacherCardSheet> TeacherCardSheets { get; set; }
    public DbSet<TeacherCardSheetActivity> TeacherCardSheetActivities { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityCategory> ActivityCategories { get; set; }
    
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This property isn't on the C# class,
        // so we set it up as a "shadow" property and use it for concurrency.
      

        modelBuilder.Entity<BaseEntity>().HasKey(be => be.Id);
        modelBuilder.Entity<BaseEntity>().Property(be => be.Id).HasConversion(
            id => id.ToString(),
            id => Ulid.Parse(id.ToString()));

        modelBuilder.Entity<Category>().ToTable("Categories");
        modelBuilder.Entity<SubjectType>().ToTable("SubjectTypes");
        modelBuilder.Entity<Faculty>().ToTable("Faculties");
        modelBuilder.Entity<Department>().ToTable("Departments");
        modelBuilder.Entity<StudyPlan>().ToTable("StudyPlans");
        modelBuilder.Entity<StudyProgram>().ToTable("StudyPrograms");
        modelBuilder.Entity<StudyForm>().ToTable("StudyForms");
        modelBuilder.Entity<StudentGroup>().ToTable("StudentGroups");
        modelBuilder.Entity<Subject>().ToTable("Subjects");
        modelBuilder.Entity<ContactHours>().ToTable("ContactHours");
        modelBuilder.Entity<NonContactHours>().ToTable("NonContactHours");
        modelBuilder.Entity<ContactHoursDetails>().ToTable("ContactHoursDetails");
        modelBuilder.Entity<NonContactHoursDetails>().ToTable("NonContactHoursDetails");

        modelBuilder.Entity<Category>()
            .HasMany(c => c.SubjectTypes)
            .WithOne(st => st.Category)
            .HasForeignKey(st => st.Id);

        modelBuilder.Entity<SubjectType>()
            .HasMany(st => st.Subjects)
            .WithOne(s => s.SubjectType)
            .HasForeignKey(s => s.Id);

        modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Departments)
            .WithOne(d => d.Faculty)
            .HasForeignKey(d => d.Id);

        modelBuilder.Entity<Department>()
            .HasMany(d => d.StudyPlans)
            .WithOne(sp => sp.Department)
            .HasForeignKey(sp => sp.Id);

        modelBuilder.Entity<Department>()
            .HasMany(d => d.StudentGroups)
            .WithOne(sg => sg.Department)
            .HasForeignKey(sg => sg.Id);

        modelBuilder.Entity<StudentGroup>()
            .HasOne(sg => sg.Faculty)
            .WithMany(f => f.StudentGroups)
            .HasForeignKey(sg => sg.Id);

        modelBuilder.Entity<Subject>()
            .HasOne(s => s.ContactHours)
            .WithOne(ch => ch.Subject)
            .HasForeignKey<ContactHours>(ch => ch.Id);

        modelBuilder.Entity<Subject>()
            .HasOne(s => s.NonContactHours)
            .WithOne(nch => nch.Subject)
            .HasForeignKey<NonContactHours>(nch => nch.Id);

        modelBuilder.Entity<ContactHours>()
            .HasOne(ch => ch.ContactHoursDetails)
            .WithOne(chd => chd.ContactHours)
            .HasForeignKey<ContactHoursDetails>(chd => chd.Id);

        modelBuilder.Entity<NonContactHours>()
            .HasOne(nch => nch.NonContactHoursDetails)
            .WithOne(nchd => nchd.NonContactHours)
            .HasForeignKey<NonContactHoursDetails>(nchd => nchd.Id);

        base.OnModelCreating(modelBuilder);
        
        ConfigureTeacherCard(modelBuilder);
        ConfigureTeacherCardSheet(modelBuilder);
        ConfigureTeacherCardSheetActivity(modelBuilder);
        ConfigureActivity(modelBuilder);
        ConfigureActivityCategory(modelBuilder);
        
        
    }
    private void ConfigureTeacherCard(ModelBuilder modelBuilder)
    {
        // TeacherCard has many TeacherCardSheets
        modelBuilder.Entity<TeacherCard>()
            .HasMany(tc => tc.Sheets)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureTeacherCardSheet(ModelBuilder modelBuilder)
    {
        // TeacherCardSheet has many TeacherCardSheetActivities
        modelBuilder.Entity<TeacherCardSheet>()
            .HasMany(tcs => tcs.Activities)
            .WithOne(tca => tca.Sheet)
            .OnDelete(DeleteBehavior.Cascade);

        // Enum conversion
        modelBuilder.Entity<TeacherCardSheet>()
            .Property(tcs => tcs.SheetType)
            .HasConversion<int>(); // Store enum as int
    }

    private void ConfigureTeacherCardSheetActivity(ModelBuilder modelBuilder)
    {
        // Each TeacherCardSheetActivity is associated with one Activity
        modelBuilder.Entity<TeacherCardSheetActivity>()
            .HasOne(tca => tca.Activity)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        // Validate HoursSpent <= MaxHours in database using a custom constraint or trigger if needed
        // OR consider using FluentValidation/validation logic in business layer instead
    }

    private void ConfigureActivity(ModelBuilder modelBuilder)
    {
        // Each Activity has one ActivityCategory
        modelBuilder.Entity<Activity>()
            .HasOne(a => a.Category)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);
    }

    private void ConfigureActivityCategory(ModelBuilder modelBuilder)
    {
        // Basic configuration for ActivityCategory
        modelBuilder.Entity<ActivityCategory>()
            .Property(ac => ac.Title)
            .HasMaxLength(100)
            .IsRequired();
    }
    
}