using Microsoft.EntityFrameworkCore;
using VikoSoft.Data;
using VikoSoft.Dto;
using VikoSoft.Entities;

namespace VikoSoft.TestFIles;

public class CreatePlanRequest
{
    public string? Label { get; set; } = string.Empty;
}

public class CreatePlan 
{
    private readonly VikoDbContext _context;

    public CreatePlan(VikoDbContext context)
    {
        _context = context;
    }

  

    public async Task HandleAsync(CreatePlanRequest req, CancellationToken ct)
    {


        var planId = Ulid.NewUlid(); 
        var planName = Ulid.NewUlid()+ req.Label;
        var plan = new Plan
        {
            Id = planId,
            Name = planName,
            Label = req.Label
        };
        _context.Plans.Add(plan); 
        await _context.SaveChangesAsync(ct); 
        
        var groups = await _context.PlannedGroups
            .Include(pg => pg.StudyProgram)
            .ThenInclude(sp => sp.Subjects)
            .Include(pg => pg.StudyProgram.Department)
            .Where(pg => pg.LabelName == "")// TODO ADD LABEL OR NOT DEPENDS
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
        
        var planSubjects = groups.SelectMany(g => g.Subjects.Select(s => new PlannedSubjectDto
        {
            Id = s.Id,
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
            PlanId = planId,
            PlannedGroupId = g.Id
        })).ToList();
        
        await SavePlannedSubjects(planSubjects, ct);
        //await SendAsync("Planas sėkmingai išsaugotas", 200, ct);
    }

    private async Task SavePlannedSubjects(List<PlannedSubjectDto> planSubjects, CancellationToken ct)
    {
        var entities = planSubjects.Select(ps => new PlannedSubject
        {
            Id = Ulid.NewUlid(),
            Name = ps.Name,
            Semester = ps.Semester,
            Credits = ps.Credits,
            EvaluationForm = ps.EvaluationForm,
            Category = ps.Category,
            CategoryDescription = ps.CategoryDescription,
            SubjectType = ps.SubjectType,
            LectureHours = ps.LectureHours,
            PracticeHours = ps.PracticeHours,
            RemoteLectureHours = ps.RemoteLectureHours,
            RemotePracticeHours = ps.RemotePracticeHours,
            SelfStudyHours = ps.SelfStudyHours,
            SubGroupsCount = ps.SubGroupsCount,
            LecturesCount = ps.LecturesCount,
            FinalProjectExamCount = ps.FinalProjectExamCount,
            OtherContactHoursCount = ps.OtherContactHoursCount,
            ConsultationCount = ps.ConsultationCount,
            GradingNumberCount = ps.GradingNumberCount,
            GradingHoursCount = ps.GradingHoursCount,
            HomeworkHoursCount = ps.HomeworkHoursCount,
            PracticeReportHoursCount = ps.PracticeReportHoursCount,
            RemoteTeachingHoursCount = ps.RemoteTeachingHoursCount,
            CourseWorkHoursCount = ps.CourseWorkHoursCount,
            ExamHours = ps.ExamHours,
            OtherNonContactCount = ps.OtherNonContactCount,
            StudyProgramId = ps.StudyProgramId,
            DepartmentId = ps.DepartmentId,
            PlanId = ps.PlanId,
            PlannedGroupId = ps.PlannedGroupId
        }).ToList();

        await _context.PlannedSubjects.AddRangeAsync(entities, ct);
        await _context.SaveChangesAsync(ct);
    }

}





