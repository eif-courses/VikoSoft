using FastEndpoints;
using StudyPlannerSoft.Data;

namespace StudyPlannerSoft.Features.Subjects;

internal sealed class ImportSubjectsRequest
{
    public IFormFile File { get; set; }
}
internal sealed class ImportSubjects(ImportSubjectsService importSubjectsService, MyDatabaseContext dbContext)
    : Endpoint<ImportSubjectsRequest>
{
    public override void Configure()
    {
        Post("/subjects/import");
        AllowFileUploads();
        AllowAnonymous();
    }

    public override async Task HandleAsync(ImportSubjectsRequest req, CancellationToken ct)
    {
        var subjects = importSubjectsService.ImportFromExcel(req.File.OpenReadStream());
        
        dbContext.Subjects.AddRange(subjects);
        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }
}