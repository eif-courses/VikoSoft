
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

    
    public DbSet<Report> Reports { get; init; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<ContactHours> ContactHours { get; set; }
    public DbSet<NonContactHours> NonContactHours { get; set; }
    public DbSet<SubjectType> SubjectTypes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This property isn't on the C# class,
        // so we set it up as a "shadow" property and use it for concurrency.
        modelBuilder.Entity<Report>()
            .Property<byte[]>(RowVersion)
            .IsRowVersion();
        
        modelBuilder.Entity<BaseEntity>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => Ulid.Parse(v.ToString()));
        
        modelBuilder.Entity<Category>()
            .HasMany(c => c.SubjectTypes)
            .WithOne(st => st.Category)
            .HasForeignKey(st => st.CategoryId)
            .IsRequired();

        // Configure SubjectType -> Subject relationship
        modelBuilder.Entity<SubjectType>()
            .HasMany(st => st.Subjects)
            .WithOne(s => s.Type)
            .HasForeignKey(s => s.TypeId)
            .IsRequired();
        
        
        
        base.OnModelCreating(modelBuilder);
    }
}