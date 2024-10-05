using Microsoft.EntityFrameworkCore;

namespace VikoSoft.Entities;

public enum StudyType
{
    Normal = 1,
    Sessions = 2,
    Remote = 3,
    English = 4
}

public class StudyProgram
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public string Name { get; set; } = string.Empty;
    
    public StudyType StudyType { get; set; } = StudyType.Normal;
    public Ulid DepartmentId { get; set; }
    public Department Department { get; set; }
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public ICollection<PlannedSubject> PlanSubjects { get; set; } = new List<PlannedSubject>();
    public ICollection<PlannedGroup> PlannedGroups { get; set; } = new List<PlannedGroup>();
    
}

public static class StudyProgramEntityConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudyProgram>()
            .HasOne(sp => sp.Department)
            .WithMany(d => d.StudyPrograms)
            .HasForeignKey(sp => sp.DepartmentId);


        modelBuilder.Entity<StudyProgram>()
            .Property(sp => sp.Name)
            .HasMaxLength(255);
    }
}