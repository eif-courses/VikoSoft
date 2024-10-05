using System.Text;
using ExcelDataReader;
using StudyPlannerSoft.Data;
using StudyPlannerSoft.Entities;

namespace StudyPlannerSoft.Features.Lecturers;

public class ImportLecturersService
{
    private readonly MyDatabaseContext _dbContext;

    public ImportLecturersService(MyDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public IEnumerable<Lecturer> ImportFromExcel(Stream excelStream)
    {
        var lecturers = new List<Lecturer>();

        using (var reader = ExcelReaderFactory.CreateReader(excelStream))
        {
            reader.NextResult(); // SECOND SHEET
            
            int currentRow = 0;

           
            while (reader.Read())
            {
                if (currentRow < 6)
                {
                    currentRow++;
                    continue;
                }
                
                var name = reader.GetValue(3)?.ToString()?.Trim();
                var email = reader.GetValue(4)?.ToString()?.Trim();
                var positionNameNormalized = reader.GetValue(5)?.ToString()?.Trim();
                var departmentNameNormalized = reader.GetValue(6)?.ToString()?.Trim();

                // Log the fetched values
                Console.WriteLine($"Row {currentRow}: Name='{name}', Email='{email}', Position='{positionNameNormalized}', Department='{departmentNameNormalized}'");

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(positionNameNormalized) || string.IsNullOrEmpty(departmentNameNormalized))
                {
                    Console.WriteLine("Skipping row due to missing data.");
                    currentRow++;
                    continue;
                }

                var department = _dbContext.Departments.FirstOrDefault(dep => dep.ShortName == departmentNameNormalized);
                
                if (department == null)
                {
                    Console.WriteLine($"Department not found for short name: {departmentNameNormalized}");
                    throw new KeyNotFoundException($"Department not found for short name: {departmentNameNormalized}");
                }

                var position = _dbContext.Positions.FirstOrDefault(pos => pos.Name == positionNameNormalized);

                if (position == null)
                {
                    Console.WriteLine($"Position not found for name: {positionNameNormalized}");
                    throw new KeyNotFoundException($"Position not found for name: {positionNameNormalized}");
                }

                var lecturer = new Lecturer
                {
                    Id = Ulid.NewUlid(),
                    Name = name,
                    Email = email,
                    PositionId = position.Id,
                    DepartmentId = department.Id
                };

                lecturers.Add(lecturer);
                currentRow++;
            }
        }

        return lecturers;
    }
}
