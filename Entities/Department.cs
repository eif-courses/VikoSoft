using Microsoft.EntityFrameworkCore;

namespace VikoSoft.Entities;

public class Department
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;

    public Ulid FacultyId { get; set; }
    public Faculty Faculty { get; set; }

    public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
    public ICollection<StudyProgram> StudyPrograms { get; set; } = new List<StudyProgram>();
    public ICollection<PlannedSubject> PlanSubjects { get; set; } = new List<PlannedSubject>();
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}

public static class DepartmentEntityConfiguration {
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>()
            .HasOne(d => d.Faculty)
            .WithMany(f => f.Departments)
            .HasForeignKey(d => d.FacultyId);
        
        modelBuilder.Entity<Department>()
            .HasMany(d => d.StudyPrograms)
            .WithOne(sp => sp.Department)
            .HasForeignKey(sp => sp.DepartmentId);

        modelBuilder.Entity<Department>()
            .Property(d => d.ShortName)
            .HasMaxLength(255);
    }
}
