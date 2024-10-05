using Microsoft.EntityFrameworkCore;

namespace StudyPlannerSoft.Entities;

public enum SubjectType
{
    Mandatory = 1,
    SpecializedElective = 2,
    Lpd = 3,
    General = 4,
    Sport = 5
}

public class Subject
{
    public Ulid Id { get; set; } = Ulid.NewUlid();
    public string Name { get; set; } = string.Empty;
    public Semester Semester { get; set; } = Semester.First;
    public int Credits { get; set; }
    public string EvaluationForm { get; set; } = string.Empty;
    public string? Category { get; set; } = string.Empty;
    public string? CategoryDescription { get; set; } = string.Empty;
    public SubjectType SubjectType { get; set; } = SubjectType.Mandatory;
   

    // Kontaktines valandos

    public double LectureHours { get; set; } 
    public double PracticeHours { get; set; }
    public double? RemoteLectureHours { get; set; }
    public double? RemotePracticeHours { get; set; }
    public double SelfStudyHours { get; set; }
    public string? Notes { get; set; } = string.Empty;

    public int SubGroupsCount { get; set; } = 1; // TODO Need TO REMOVE OR LEAVE  
    public int LecturesCount { get; set; } 
    public double FinalProjectExamCount { get; set; }
    public double? OtherContactHoursCount { get; set; }
    public double ConsultationCount { get; set; }

    // Ne kontaktines valandos

    public double GradingNumberCount { get; set; }
    public double? GradingHoursCount { get; set; }
    
    public double? HomeworkHoursCount { get; set; }
    public double? PracticeReportHoursCount { get; set; }
    public double? RemoteTeachingHoursCount { get; set; }
    public double? CourseWorkHoursCount { get; set; }
    public double? ExamHours { get; set; }
    
    public double? OtherNonContactCount { get; set; }
    
    public Ulid StudyProgramId { get; set; } 
    public StudyProgram StudyProgram { get; set; }
    
    public Ulid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();

    
}

public static class SubjectEntityConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Subject>()
            .HasOne(s => s.StudyProgram)
            .WithMany(sp => sp.Subjects)
            .HasForeignKey(s => s.StudyProgramId);
        
        modelBuilder.Entity<Subject>()
            .HasOne(s => s.Department)
            .WithMany(d => d.Subjects)
            .HasForeignKey(s => s.DepartmentId)
            .IsRequired(false); 
        
        modelBuilder.Entity<Subject>()
            .HasMany(s => s.Lecturers)
            .WithMany(l => l.Subjects)
            .UsingEntity(j => j.ToTable("SubjectLecturers"));
        
        modelBuilder.Entity<Subject>()
            .Property(d => d.Name)
            .HasMaxLength(255);
    }
}