using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using StudyPlannerSoft.Data;
using StudyPlannerSoft.Dto;
using StudyPlannerSoft.Entities;

namespace StudyPlannerSoft.Features.Groups;

public class RequestGroupsByLabel
{
    public string Label { get; set; } = string.Empty; // The label to filter by
}

public class ListAllGroupsByLabel : Endpoint<RequestGroupsByLabel, List<PlannedGroupDto>>
{
    private readonly MyDatabaseContext _context;

    public ListAllGroupsByLabel(MyDatabaseContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/groups/all"); // Define the endpoint route
        AllowAnonymous(); // Allow anonymous access
    }

    public override async Task HandleAsync(RequestGroupsByLabel req, CancellationToken ct)
    {
        var groups = await _context.PlannedGroups
            .Include(pg => pg.StudyProgram)
            .ThenInclude(sp => sp.Subjects)
            .Include(pg => pg.StudyProgram.Department)
            .Where(pg => pg.LabelName == "") // NEED TO ADD req.label
            .Select(pg => new PlannedGroupDto
            {
                Id = pg.Id,
                Name = pg.Name,
                Semester = pg.Semester,
                LabelName = pg.LabelName,
                Vf = pg.Vf,
                Vnf = pg.Vnf,
                StudyProgramId = pg.StudyProgramId,
                LecturerId = pg.LecturerId,
                SubGroupCount = pg.SubGroupCount,
                Subjects = pg.StudyProgram.Subjects
                    .Where(s =>
                        (pg.Semester == Semester.First &&
                         (s.Semester == Semester.First || s.Semester == Semester.Second)) ||
                        (pg.Semester == Semester.Third &&
                         (s.Semester == Semester.Third || s.Semester == Semester.Fourth)) ||
                        (pg.Semester == Semester.Fifth &&
                         (s.Semester == Semester.Fifth || s.Semester == Semester.Sixth)) ||
                        (pg.Semester == Semester.Seventh && s.Semester == Semester.Seventh))
                    .Select(s => new SubjectDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Semester = s.Semester,
                        Credits = s.Credits,
                        EvaluationForm = s.EvaluationForm,
                        Category = s.Category,
                        SubjectType = s.SubjectType,
                        ConsultationCount = s.ConsultationCount,
                        ExamHours = s.ExamHours,
                        HomeworkHoursCount = s.HomeworkHoursCount,
                        LectureHours = s.LectureHours,
                        CourseWorkHoursCount = s.CourseWorkHoursCount,
                        FinalProjectExamCount = s.FinalProjectExamCount,
                        OtherContactHoursCount = s.OtherContactHoursCount,
                        OtherNonContactCount = s.OtherNonContactCount,
                        PracticeReportHoursCount = s.PracticeReportHoursCount,
                        RemoteTeachingHoursCount = s.RemoteTeachingHoursCount,
                        LecturesCount = s.LecturesCount,
                        StudyProgramId = s.StudyProgramId,
                        SubGroupsCount = s.SubGroupsCount,
                        CategoryDescription = s.CategoryDescription,
                        PracticeHours = s.PracticeHours,
                        GradingHoursCount = s.GradingHoursCount,
                        GradingNumberCount = s.GradingNumberCount,
                        RemoteLectureHours = s.RemoteLectureHours,
                        RemotePracticeHours = s.RemotePracticeHours,
                        SelfStudyHours = s.SelfStudyHours,
                        DepartmentId = pg.StudyProgram.DepartmentId
                    }).ToList()
            })
            .ToListAsync(ct);

        var planSubjects = groups.SelectMany(g => g.Subjects.Select(s => new PlannedSubject
        {
            Id = s.Id, // Include the Id property
            Name = s.Name,
            Semester = s.Semester,
            Credits = s.Credits,
            EvaluationForm = s.EvaluationForm,
            Category = s.Category,
            CategoryDescription = s.CategoryDescription,
            SubjectType = s.SubjectType,
            LectureHours = s.LectureHours,
            PracticeHours = s.PracticeHours,
            RemoteLectureHours = s.RemoteLectureHours,
            RemotePracticeHours = s.RemotePracticeHours,
            SelfStudyHours = s.SelfStudyHours,
            SubGroupsCount = s.SubGroupsCount,
            LecturesCount = s.LecturesCount,
            FinalProjectExamCount = s.FinalProjectExamCount,
            OtherContactHoursCount = s.OtherContactHoursCount,
            ConsultationCount = s.ConsultationCount,
            GradingNumberCount = s.GradingNumberCount,
            GradingHoursCount = s.GradingHoursCount,
            HomeworkHoursCount = s.HomeworkHoursCount,
            PracticeReportHoursCount = s.PracticeReportHoursCount,
            RemoteTeachingHoursCount = s.RemoteTeachingHoursCount,
            CourseWorkHoursCount = s.CourseWorkHoursCount,
            ExamHours = s.ExamHours,
            OtherNonContactCount = s.OtherNonContactCount,
            StudyProgramId = s.StudyProgramId,
            DepartmentId = s.DepartmentId,
            // PlanId = 
        })).ToList();

        // Save the planned subjects to the database
        await SavePlannedSubjects(planSubjects, ct);

        await SendAsync(groups, 200, ct);
    }
    private async Task SavePlannedSubjects(IEnumerable<PlannedSubject> planSubjects, CancellationToken ct)
    {
        // Assuming you have a DbSet<PlanSubject> in your DbContext
        // for example: public DbSet<PlanSubject> PlanSubjects { get; set; }
    
        foreach (var planSubject in planSubjects)
        {
            // Add the plan subject to the context
            await _context.PlannedSubjects.AddAsync(planSubject, ct);
        }

        // Save all changes in the context to the database
        await _context.SaveChangesAsync(ct);
    }
    
}

// public class PlannedGroupDto
// {
//     public Ulid Id { get; set; } = Ulid.NewUlid();
//     public string Name { get; set; } = string.Empty;
//     public Semester Semester { get; set; }
//     public string? LabelName { get; set; } // If needed for your use case
//
//     public int? Vf { get; set; } = 0;
//     public int? Vnf { get; set; } = 0;
//     public string? SubGroupCount { get; set; } = "1";
//     public Ulid StudyProgramId { get; set; }
//     public Ulid? LecturerId { get; set; }
//
//     public List<SubjectDto> Subjects { get; set; } = new List<SubjectDto>();
// }

// public class SubjectDto
// {
//     public Ulid Id { get; set; } = Ulid.NewUlid();
//     public string Name { get; set; } = string.Empty;
//     public Semester Semester { get; set; } = Semester.First;
//     public int Credits { get; set; }
//     public string EvaluationForm { get; set; } = string.Empty;
//     public string? Category { get; set; } = string.Empty;
//
//     public string? CategoryDescription { get; set; } = string.Empty;
//     public SubjectType SubjectType { get; set; } = SubjectType.Mandatory;
//
//     public double LectureHours { get; set; }
//     public double PracticeHours { get; set; }
//     public double? RemoteLectureHours { get; set; }
//     public double? RemotePracticeHours { get; set; }
//     public double SelfStudyHours { get; set; }
//
//     public int SubGroupsCount { get; set; } = 1;
//     public int LecturesCount { get; set; }
//     public double FinalProjectExamCount { get; set; }
//     public double? OtherContactHoursCount { get; set; }
//     public double ConsultationCount { get; set; }
//
//     // Ne kontaktines valandos
//
//     public double GradingNumberCount { get; set; }
//     public double? GradingHoursCount { get; set; }
//     public double? HomeworkHoursCount { get; set; }
//     public double? PracticeReportHoursCount { get; set; }
//     public double? RemoteTeachingHoursCount { get; set; }
//     public double? CourseWorkHoursCount { get; set; }
//     public double? ExamHours { get; set; }
//     public double? OtherNonContactCount { get; set; }
//     public Ulid StudyProgramId { get; set; }
//
//     public Ulid? DepartmentId { get; set; }
// }