using Microsoft.EntityFrameworkCore;

namespace VikoSoft.Entities;

public class Lecturer
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public Ulid PositionId { get; set; }
    public Position Position { get; set; }

    public Ulid DepartmentId { get; set; }
    public Department Department { get; set; }
    
    public ICollection<PlannedGroup> PlannedGroups { get; set; } = new List<PlannedGroup>();
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    
}

public static class LecturerEntityConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lecturer>()
            .HasOne(l => l.Position)
            .WithMany()
            .HasForeignKey(l => l.PositionId);
        
        modelBuilder.Entity<Lecturer>()
            .HasOne(l => l.Department)
            .WithMany(d => d.Lecturers)
            .HasForeignKey(l => l.DepartmentId);
        
      
    }
}
