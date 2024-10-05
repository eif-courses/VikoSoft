using VikoSoft.Data;
using VikoSoft.Services;

namespace VikoSoft.TestFIles;

internal sealed class ImportSubjectsRequest
{
    public IFormFile File { get; set; }
}
internal sealed class ImportSubjects(ImportSubjectsService importSubjectsService, VikoDbContext dbContext)
{
  

    public async Task HandleAsync(ImportSubjectsRequest req, CancellationToken ct)
    {
        var subjects = importSubjectsService.ImportFromExcel(req.File.OpenReadStream());
        dbContext.Subjects.AddRange(subjects);
        await dbContext.SaveChangesAsync(ct);

        //await SendOkAsync(ct);
    }
}