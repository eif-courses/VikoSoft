using FastEndpoints;
using StudyPlannerSoft.Data;

namespace StudyPlannerSoft.Features.Lecturers;

internal sealed class ImportLecturersRequest
{
    public IFormFile File { get; set; }
}



internal sealed class ImportLecturers : Endpoint<ImportLecturersRequest>
{
    private readonly MyDatabaseContext _dbContext;
    private readonly ImportLecturersService _importLecturersService;
    private readonly ILogger<ImportLecturers> _logger;

    public ImportLecturers(MyDatabaseContext dbContext, ImportLecturersService importLecturersService,
        ILogger<ImportLecturers> logger)
    {
        _dbContext = dbContext;
        _importLecturersService = importLecturersService;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("lecturers/import");
        AllowFileUploads();
        AllowAnonymous();
    }

    public override async Task HandleAsync(ImportLecturersRequest req, CancellationToken ct)
    {
        var lecturers = _importLecturersService.ImportFromExcel(req.File.OpenReadStream());
        _dbContext.Lecturers.AddRange(lecturers);
        
        await _dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }
}
