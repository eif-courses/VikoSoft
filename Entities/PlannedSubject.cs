using Microsoft.EntityFrameworkCore;

namespace StudyPlannerSoft.Entities;

public class PlannedSubject
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
    public Ulid? PlanId { get; set; }
    public Plan? Plan { get; set; }
    
    public Ulid? PlannedGroupId { get; set; }
    public PlannedGroup? PlannedGroup { get; set; }
    
    
}
public static class PlanSubjectEntityConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
            modelBuilder.Entity<PlannedSubject>()
                .HasOne(x => x.Plan)
                .WithMany(s => s.PlanSubjects)
                .HasForeignKey(p => p.PlanId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<PlannedSubject>()
                .HasOne(s => s.StudyProgram)
                .WithMany(x => x.PlanSubjects)
                .HasForeignKey(st => st.StudyProgramId);
            
            modelBuilder.Entity<PlannedSubject>()
                .HasOne(s => s.PlannedGroup)
                .WithMany(x => x.PlanSubjects)
                .HasForeignKey(st => st.PlannedGroupId);
            
           modelBuilder.Entity<PlannedSubject>()
               .HasOne(s => s.Department)
               .WithMany(x => x.PlanSubjects)
               .HasForeignKey(st => st.DepartmentId);
            
    }
}