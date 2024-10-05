using Microsoft.EntityFrameworkCore;

namespace VikoSoft.Entities;

public class PlannedGroup
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public string Name { get; set; } = string.Empty; // Jeigu yra per kableli reiskia yra srautas
    public Semester Semester { get; set; }
    public int? Vf { get; set; } = 0;
    public int? Vnf { get; set; } = 0;
    public string? SubGroupCount { get; set; } = "1";
    public int? OtherType { get; set; } = 0;
    public string? LabelName { get; set; } = string.Empty;

    public Ulid StudyProgramId { get; set; }
    public StudyProgram StudyProgram { get; set; }

    public Ulid? LecturerId { get; set; }
    public Lecturer? Lecturer { get; set; }
    
    
    public ICollection<PlannedSubject> PlanSubjects { get; set; } = new List<PlannedSubject>();
    
    
}

public static class PlannedGroupEntityConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlannedGroup>(entity =>
        {
            entity.Property(e => e.SubGroupCount)
                .HasMaxLength(10)
                .HasDefaultValue("1");

            entity.HasOne(e => e.StudyProgram)
                .WithMany(sp => sp.PlannedGroups)
                .HasForeignKey(e => e.StudyProgramId);

            entity.HasOne(e => e.Lecturer)
                .WithMany(sub => sub.PlannedGroups)
                .HasForeignKey(p => p.LecturerId);
        });
    }
}