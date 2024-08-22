
using Tarifikacija.Entities;

namespace VikoSoft.Data.Entities;



public class Category : BaseEntity
{
    public string Title { get; set; }
    public ICollection<SubjectType> SubjectTypes { get; set; } = new List<SubjectType>();
}

public class SubjectType : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public Category Category { get; set; }
}

public class Faculty : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
}

public class Department : BaseEntity
{
    public string Name { get; set; }
    public Faculty Faculty { get; set; }
    public ICollection<StudyPlan> StudyPlans { get; set; } = new List<StudyPlan>();
    public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
}

public class StudyPlan : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Department Department { get; set; } = null!;
}

public class StudyProgram : BaseEntity
{
    public string Name { get; set; }
}

public class StudyForm : BaseEntity
{
    public string Name { get; set; }
}

public class StudentGroup : BaseEntity
{
    public string Name { get; set; }
    public int Semester { get; set; }
    public int StudentNumber { get; set; }
    public int Vf { get; set; }
    public int Vnf { get; set; }
    public int Year { get; set; }
    public Department Department { get; set; } = null!;
    public Faculty Faculty { get; set; } = null!;
}

public class Subject : BaseEntity
{
    public string Title { get; set; }
    public int Credits { get; set; }
    public int Semester { get; set; }
    public ContactHours ContactHours { get; set; }
    public NonContactHours NonContactHours { get; set; }
    public SubjectType SubjectType { get; set; }
}

public class ContactHours : BaseEntity
{
    public int LectureHours { get; set; }
    public int PracticeHours { get; set; }
    public int? RemoteLectureHours { get; set; }
    public int? RemotePracticeHours { get; set; }
    public int SelfStudyHours { get; set; }
    public string Notes { get; set; }
    public ContactHoursDetails ContactHoursDetails { get; set; }
    public Subject Subject { get; set; }
}

public class NonContactHours : BaseEntity
{
    public NonContactHoursDetails NonContactHoursDetails { get; set; }
    public Subject Subject { get; set; }
}

public class ContactHoursDetails : BaseEntity
{
    public int SubGroupsCount { get; set; }
    public int LecturesCount { get; set; }
    public int FinalProjectExamCount { get; set; }
    public int OtherCount { get; set; }
    public int ConsultationCount { get; set; }
    public int TotalContactHours { get; set; }
    public ContactHours ContactHours { get; set; }
}

public class NonContactHoursDetails : BaseEntity
{
    public int GradingNumberCount { get; set; }
    public int GradingHours { get; set; }
    public int OtherCount { get; set; }
    public NonContactHours NonContactHours { get; set; }
}




