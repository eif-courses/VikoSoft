using System.Text;
using ExcelDataReader;
using VikoSoft.Data;
using VikoSoft.Entities;

namespace VikoSoft.Services;

public class ImportGroupsService
{
    private readonly VikoDbContext _dbContext;

    public ImportGroupsService(VikoDbContext dbContext)
    {
        _dbContext = dbContext;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); 
    }

    public IEnumerable<PlannedGroup> ImportFromExcel(Stream excelStream)
    {
        var groups = new List<PlannedGroup>();

        // Use ExcelDataReader to read the Excel file
        using (var reader = ExcelReaderFactory.CreateReader(excelStream))
        {
            // Skip to the second sheet (if needed)
            reader.NextResult(); // Move to the first sheet
            reader.NextResult(); // Move to the second sheet

            int currentRow = 0;

            // Read through the rows of the Excel sheet
            while (reader.Read())
            {
                // Skip rows before the header row (e.g., first 6 rows)
                if (currentRow < 6)
                {
                    currentRow++;
                    continue;
                }

                // Normalize study program type and name
                var studyProgramType = Enum.TryParse<StudyType>(reader.GetValue(7)?.ToString(), out var pType)
                    ? pType
                    : StudyType.Normal;
                var studyProgram = reader.GetValue(8)?.ToString()?.Trim();

                // Find the study program from the database
                var studyProgramResult = _dbContext.StudyPrograms
                    .FirstOrDefault(st => st.StudyType == studyProgramType && st.Name == studyProgram);

                if (studyProgramResult == null)
                {
                    Console.WriteLine($"Study program not found: {studyProgram}");
                    throw new Exception("Study program not found");
                }
            
                // Create and populate a new PlannedGroup object
                var group = new PlannedGroup
                {
                    Id = Ulid.NewUlid(),
                    Name = reader.GetValue(3)?.ToString()?.Trim(), // Name of the planned group
                    Semester = Enum.TryParse<Semester>(reader.GetValue(4)?.ToString(), out var semester)
                        ? semester
                        : Semester.First, // Default to First semester if parsing fails
                    Vf = int.TryParse(reader.GetValue(5)?.ToString(), out var vf) ? vf : 0, // Validate and parse Vf
                    Vnf = int.TryParse(reader.GetValue(6)?.ToString(), out var vnf) ? vnf : 0, // Validate and parse Vnf
                    StudyProgramId = studyProgramResult.Id // Foreign key to the study program
                };

                groups.Add(group); // Add the group to the list
                currentRow++;
            }
        }

        return groups; // Return the list of groups
    }
}
