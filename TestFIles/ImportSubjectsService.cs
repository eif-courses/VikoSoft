using System.Text;
using ExcelDataReader;
using StudyPlannerSoft.Data;
using StudyPlannerSoft.Entities;

namespace StudyPlannerSoft.Features.Subjects;

public class ImportSubjectsService
{
    private readonly MyDatabaseContext _dbContext;

    public ImportSubjectsService(MyDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Required to support older Excel formats
    }

    public IEnumerable<Subject> ImportFromExcel(Stream excelStream)
    {
        var subjects = new List<Subject>();

        using (var reader = ExcelReaderFactory.CreateReader(excelStream))
        {
            int currentRow = 0;

            // Read through each row of the Excel file
            while (reader.Read())
            {
                currentRow++;

                // Start processing from row 7, assuming the first 6 rows are headers or irrelevant data
                if (currentRow < 7) continue;

                // Extract data from columns
                string studyProgramName = reader.GetValue(0)?.ToString()?.Trim();
                string studyProgramType = reader.GetValue(1)?.ToString()?.Trim();

                // Skip empty lines
                if (string.IsNullOrEmpty(studyProgramName) && string.IsNullOrEmpty(studyProgramType))
                {
                    continue; // Skip empty rows
                }

                // Validate and convert timetable type
                if (string.IsNullOrEmpty(studyProgramType) ||
                    !Enum.TryParse(studyProgramType, out StudyType timetableType))
                {
                    Console.WriteLine($"Invalid timetable type '{studyProgramType}' in row {currentRow}.");
                    continue; // Skip this row instead of throwing an exception
                }

                // Find the corresponding StudyProgram
                var studyProgram = _dbContext.StudyPrograms
                    .FirstOrDefault(sp => sp.Name == studyProgramName && sp.StudyType == timetableType);

                if (studyProgram == null)
                {
                    Console.WriteLine($"StudyProgram '{studyProgramName}' not found in row {currentRow}.");
                    continue; // Skip to the next row if the study program is not found
                }

                // Create a Subject entity and parse the remaining columns
                var subject = new Subject
                {
                    Name = reader.GetValue(2)?.ToString()?.Trim(),
                    Semester = Enum.TryParse<Semester>(reader.GetValue(3)?.ToString()?.Trim(), out var semester)
                        ? semester
                        : Semester.First,
                    SubjectType =
                        Enum.TryParse<SubjectType>(reader.GetValue(4)?.ToString()?.Trim(), out var subjectType)
                            ? subjectType
                            : SubjectType.Mandatory,
                    LectureHours = double.TryParse(reader.GetValue(5)?.ToString(), out var lectureHours)
                        ? lectureHours
                        : 0,
                    PracticeHours = double.TryParse(reader.GetValue(6)?.ToString(), out var practiceHours)
                        ? practiceHours
                        : 0,
                    RemoteLectureHours = double.TryParse(reader.GetValue(7)?.ToString(), out var remoteLectureHours)
                        ? remoteLectureHours
                        : 0,
                    RemotePracticeHours = double.TryParse(reader.GetValue(8)?.ToString(), out var remotePracticeHours)
                        ? remotePracticeHours
                        : 0,
                    SelfStudyHours = double.TryParse(reader.GetValue(9)?.ToString(), out var selfStudyHours)
                        ? selfStudyHours
                        : 0,
                    Credits = int.TryParse(reader.GetValue(10)?.ToString(), out var credits)
                        ? credits
                        : 0,
                    EvaluationForm = reader.GetValue(11)?.ToString()?.Trim(),
                    Category = reader.GetValue(12)?.ToString()?.Trim(),
                    CategoryDescription = reader.GetValue(13)?.ToString()?.Trim(),
                    FinalProjectExamCount =
                        double.TryParse(reader.GetValue(14)?.ToString(), out var finalProjectExamCount)
                            ? finalProjectExamCount
                            : 0,
                    OtherContactHoursCount =
                        double.TryParse(reader.GetValue(15)?.ToString(), out var otherContactHoursCount)
                            ? otherContactHoursCount
                            : 0,
                    ConsultationCount = double.TryParse(reader.GetValue(16)?.ToString(), out var consultationCount)
                        ? consultationCount
                        : 0,
                    GradingNumberCount = double.TryParse(reader.GetValue(17)?.ToString(), out var gradingNumberCount)
                        ? gradingNumberCount
                        : 0,
                    HomeworkHoursCount = double.TryParse(reader.GetValue(18)?.ToString(), out var homeworkHoursCount)
                        ? homeworkHoursCount
                        : 0,
                    PracticeReportHoursCount =
                        double.TryParse(reader.GetValue(19)?.ToString(), out var practiceReportHoursCount)
                            ? practiceReportHoursCount
                            : 0,
                    RemoteTeachingHoursCount =
                        double.TryParse(reader.GetValue(20)?.ToString(), out var remoteTeachingHoursCount)
                            ? remoteTeachingHoursCount
                            : 0,
                    CourseWorkHoursCount =
                        double.TryParse(reader.GetValue(21)?.ToString(), out var courseWorkHoursCount)
                            ? courseWorkHoursCount
                            : 0,
                    ExamHours = double.TryParse(reader.GetValue(22)?.ToString(), out var examHours)
                        ? examHours
                        : 0,
                    OtherNonContactCount =
                        double.TryParse(reader.GetValue(23)?.ToString(), out var otherNonContactCount)
                            ? otherNonContactCount
                            : 0,
                    StudyProgramId = studyProgram.Id,
                };

                subjects.Add(subject);
            }
        }

        return subjects;
    }
}